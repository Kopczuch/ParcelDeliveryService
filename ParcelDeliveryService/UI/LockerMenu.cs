using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using ParcelDeliveryService.Repositories;

namespace ParcelDeliveryService.UI
{
    public class LockerMenu : IMenu
    {
        private readonly ILockerService _lockerService;
        private readonly IParcelService _parcelService;
        private readonly ILockerRepository _lockerRepository;

        public LockerMenu(
            ILockerService lockerService,
            IParcelService parcelService,
            ILockerRepository lockerRepository)
        {
            _lockerService = lockerService;
            _parcelService = parcelService;
            _lockerRepository = lockerRepository;
        }

        public void Run()
        {
            while (true)
            {
                var requestedOperation = DisplayMenu();

                if (int.TryParse(requestedOperation, out var operation))
                {
                    switch (operation)
                    {
                        case 1:
                            DepositParcel();
                            break;

                        case 2:
                            ReceiveParcel();
                            break;

                        case 3:
                            ChangeAddress();
                            break;

                        case 4:
                            ReceiveParcelFromExternalStorage();
                            break;
                        case 5:
                            DisplayParcelHistory();
                            break;
                        case 6:
                            ExpandLocker();
                            break;

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again. Press any key to continue...");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number. Press any key to continue...");
                    Console.ReadLine();
                }
            }
        }

        private string? DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("[1] Deposit Parcel");
            Console.WriteLine("[2] Receive Parcel");
            Console.WriteLine("[3] Change Locker Address");
            Console.WriteLine("[4] Receive Parcel From External Storage");
            Console.WriteLine("[5] Display Parcel History [In Locker]");
            Console.WriteLine("[6] Expand Locker");

            Console.WriteLine("[0] Go Back");
            Console.WriteLine();
            Console.Write("Choose Operation: ");
            return Console.ReadLine();
        }

        private void DisplayParcelHistory()
        {
            try
            {
                Console.Clear();
                var lockers = _lockerService.GetLockers();

                foreach (var locker in lockers)
                {
                    Console.WriteLine($"Locker #{locker.Id}");
                    foreach (var historyItem in locker.LockerHistory)
                    {
                        historyItem.Display();
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message} Press any key to continue...");
                Console.ReadLine();
            }
        }

        private void DepositParcel()
        {
            try
            {
                Console.Clear();
                Console.Write("Provide parcel ID: ");
                if (!int.TryParse(Console.ReadLine(), out var parcelId))
                {
                    Console.WriteLine("Invalid parcel ID. Please enter a valid number. Press any key to continue...");
                    Console.ReadLine();

                    return;
                }

                var parcel = _parcelService.GetParcel(parcelId);
                if (parcel == null)
                {
                    Console.WriteLine($"Parcel with ID {parcelId} not found. Press any key to continue...");
                    Console.ReadLine();

                    return;
                }

                if (parcel.CurrentState != TransitEventType.Registered)
                {
                    Console.WriteLine("Package was already deposited. Press any key to continue...");
                    Console.ReadLine();

                    return;
                }

                var availableLockers = _lockerService.GetVacantLockers();
                if (!availableLockers.Any())
                {
                    Console.WriteLine("No available lockers. Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                DisplayAvailableLockers(availableLockers);

                Console.WriteLine();
                Console.Write("Pass chosen locker ID: ");
                if (!int.TryParse(Console.ReadLine(), out var chosenLockerId))
                {
                    Console.WriteLine("Invalid locker ID. Please enter a valid number. Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                _lockerService.DepositParcel(parcel, chosenLockerId);

                parcel.SenderLockerId = chosenLockerId;

                var command = new DepositParcelCommand(_parcelService);
                command.Execute(parcel);

                Console.WriteLine();
                Console.WriteLine("Parcel deposit successful. Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message} Press any key to continue...");
                Console.ReadLine();
            }
        }

        private void ReceiveParcel()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Go to your locker.");
                var lockers = _lockerService.GetLockers();

                foreach (var locker in lockers)
                {
                    Console.WriteLine();
                    locker.Display();
                }

                Console.Write("Locker Id: ");
                var lockerId = int.Parse(Console.ReadLine());

                Console.Clear();
                Console.WriteLine($"Locker #{lockerId}\n");
                Console.Write("Provide parcel ID: ");

                var parcelId = int.Parse(Console.ReadLine());
                var parcel = _parcelService.GetParcel(parcelId);

                if (parcel == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Parcel not found");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();

                    return;
                }

                Console.Clear();

                if (parcel.CurrentState is TransitEventType.InExternalStorage or TransitEventType.DeadlineOver)
                {
                    Console.WriteLine("Parcel was not picked up during the mandatory 48 hours curfew.");

                    if (!parcel.CanExtend)
                    {
                        Console.WriteLine("You can no longer extend parcel receiving period. Press any key to continue...");
                        Console.ReadLine();

                        return;
                    }

                    parcel.CalculateAdditionalFee();

                    Console.WriteLine(parcel.CurrentState is TransitEventType.DeadlineOver
                        ? "Parcel is still in the locker."
                        : "It currently resides in our external storage");

                    Console.WriteLine("You can still receive your parcel after paying additional fee.");
                    Console.WriteLine($"Do you want to pay ${parcel.AdditionalCosts} to receive your parcel? [y/n]:");

                    var decision = Console.ReadLine()?.ToLower();

                    if (decision != "y")
                        return;

                    parcel.CanExtend = false;

                    if (parcel.CurrentState is TransitEventType.DeadlineOver)
                        PickUpParcel(parcel);
                    else
                        ExtendDeadline(parcel);

                    return;
                }

                var result = _lockerService.ReceiveFromLocker(parcelId, lockerId);

                if (result)
                {
                    PickUpParcel(parcel);
                    Console.WriteLine("Parcel received successfully.");
                }
                else
                {
                    Console.WriteLine("Parcel has not arrived yet.");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message} Press any key to continue...");
                Console.ReadLine();
            }
        }

        private void ChangeAddress()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Choose your locker.");
                var lockers = _lockerService.GetLockers();

                foreach (var locker in lockers)
                {
                    Console.WriteLine();
                    locker.Display();
                }

                Console.Write("Locker Id: ");
                if (!int.TryParse(Console.ReadLine(), out var lockerId))
                {
                    Console.WriteLine("Invalid locker ID. Please enter a valid number. Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                if (lockers.All(l => l.Id != lockerId))
                {
                    Console.WriteLine("No locker with this id. Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                Console.Clear();
                Console.WriteLine($"Locker #{lockerId}\n");

                Console.WriteLine("Enter New Address Details:");
                Console.Write("Country: ");
                string country = Console.ReadLine();

                Console.Write("City: ");
                string city = Console.ReadLine();

                Console.Write("Zip Code: ");
                string zipCode = Console.ReadLine();

                Console.Write("Street: ");
                string street = Console.ReadLine();

                Console.Write("Apartment number: ");
                string apartment = Console.ReadLine();

                Address address = new Address(country, city, zipCode, street, apartment);

                bool result = _lockerService.ChangeAddress(lockerId, address);
                Console.WriteLine(result ? "Locker's address changed successfully." : "Locker is currently in use, so it cannot be moved.");

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message} Press any key to continue...");
                Console.ReadLine();
            }
        }

        private void ReceiveParcelFromExternalStorage()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Pass parcel id: ");
                if (!int.TryParse(Console.ReadLine(), out var externalStorageParcelId))
                {
                    Console.WriteLine("Invalid parcel ID. Please enter a valid number. Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                var parcels = _parcelService.ListParcels();

                var externalStorageParcel = parcels.FirstOrDefault(p =>
                        p.Id == externalStorageParcelId && p.CurrentState == TransitEventType.DeadlineExtended);

                if (externalStorageParcel == null)
                {
                    Console.WriteLine("No eligible parcels to be received from external storage. Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                PickUpParcel(externalStorageParcel);
                Console.WriteLine("Parcel picked up from external storage. Press any key to continue...");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message} Press any key to continue...");
                Console.ReadLine();
            }
        }

        private void DisplayAvailableLockers(IEnumerable<Locker> lockers)
        {
            foreach (var locker in lockers)
            {
                Console.WriteLine();
                locker.Display();
            }
        }

        private void PickUpParcel(Parcel parcel)
        {
            var command = new PickUpParcelCommand(_parcelService);
            command.Execute(parcel);
        }

        private void ExtendDeadline(Parcel parcel)
        {
            var command = new ExtendParcelDeadlineCommand(_parcelService);
            command.Execute(parcel);
        }

        private void ExpandLocker()
        {
            Console.Clear();
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select a locker to expand:");
                Console.ResetColor();

                var lockers = _lockerService.GetLockers();
                foreach (var locker in lockers)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{locker.Id}] Locker #{locker.Id}");
                    Console.ResetColor();
                }


                Console.Write("Enter locker ID: ");
                if (!int.TryParse(Console.ReadLine(), out var lockerId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid locker ID.");
                    Console.ResetColor();
                    return;
                }

                var selectedLocker = lockers.FirstOrDefault(l => l.Id == lockerId);
                if (selectedLocker == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid locker ID. Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    return;
                }

                Console.Write("Enter the number of slots to add: ");
                if (!int.TryParse(Console.ReadLine(), out var slotsToAdd))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid number of slots. Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    return;
                }

                var compositeLocker = new LockerComposite(selectedLocker);
                for (int i = 0; i < slotsToAdd; i++)
                {
                    Console.WriteLine($"Slot {i + 1}:");
                    Console.Write("Enter slot size (Small, Medium, Large): ");
                    if (!Enum.TryParse<Size>(Console.ReadLine(), out var size))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid slot size. Operation cancelled.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                        return;
                    }
                    compositeLocker.AddSlot(new Slot { Size = size, Vacancy = VacancyState.Vacant });
                }

                foreach (var slot in compositeLocker.AdditionalSlots)
                {
                    selectedLocker.Slots.Add(slot);
                }

                _lockerRepository.Update(selectedLocker);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Locker #{lockerId} expanded successfully. Press any key to continue...");
                Console.ResetColor();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while expanding the locker: {ex.Message} Press any key to continue...");
                Console.ResetColor();
                Console.ReadLine();
            }
        }
    }
}