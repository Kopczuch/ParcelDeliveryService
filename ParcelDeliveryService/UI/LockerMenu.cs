using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.UI
{
    public class LockerMenu : IMenu
    {
        private readonly ILockerService _lockerService;
        private readonly IParcelService _parcelService;
        private readonly IParcelRepository _parcelRepository;

        public LockerMenu(
            ILockerService lockerService,
            IParcelService parcelService,
            IParcelRepository parcelRepository)
        {
            _lockerService = lockerService;
            _parcelService = parcelService;
            _parcelRepository = parcelRepository;   
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

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private string? DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("[1] Deposit Parcel");
            Console.WriteLine("[2] Receive Parcel");
            Console.WriteLine("[0] Go Back");
            Console.WriteLine();
            Console.Write("Choose Operation: ");
            return Console.ReadLine();
        }

        private void DepositParcel()
        {
            try
            {
                Console.Clear();
                Console.Write("Provide parcel ID: ");
                if (!int.TryParse(Console.ReadLine(), out var parcelId))
                {
                    Console.WriteLine("Invalid parcel ID. Please enter a valid number.");
                    return;
                }

                var parcel = _parcelService.GetParcel(parcelId);
                if (parcel == null)
                {
                    Console.WriteLine($"Parcel with ID {parcelId} not found.");
                    return;
                }

                var availableLockers = _lockerService.GetVacantLockers();
                if (!availableLockers.Any())
                {
                    Console.WriteLine("No available lockers.");
                    return;
                }

                DisplayAvailableLockers(availableLockers);

                Console.WriteLine();
                Console.Write("Pass chosen locker ID: ");
                if (!int.TryParse(Console.ReadLine(), out var chosenLockerId))
                {
                    Console.WriteLine("Invalid locker ID. Please enter a valid number.");
                    return;
                }

                _lockerService.DepositParcel(parcel, chosenLockerId);

                parcel.SenderLockerId = chosenLockerId;
                parcel.AddDepositEvent();

                _parcelRepository.Update(parcel);

                Console.WriteLine();
                Console.WriteLine("Parcel deposit successful. Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void ReceiveParcel()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Go to your locker.");
                var lockers = _lockerService.GetLockers();
                if (!lockers.Any())
                {
                    Console.WriteLine("No lockers available.");
                    return;
                }

                DisplayAvailableLockers(lockers);

                Console.Write("Locker Id: ");
                if (!int.TryParse(Console.ReadLine(), out var lockerId))
                {
                    Console.WriteLine("Invalid locker ID. Please enter a valid number.");
                    return;
                }

                Console.Clear();
                Console.WriteLine($"Locker #{lockerId}\n");
                Console.Write("Provide parcel ID: ");
                if (!int.TryParse(Console.ReadLine(), out var parcelId))
                {
                    Console.WriteLine("Invalid parcel ID. Please enter a valid number.");
                    return;
                }

                var result = _lockerService.ReceiveFromLocker(parcelId, lockerId);
                _parcelService.PickUp(parcelId);

                Console.Clear();
                Console.WriteLine(result ? "Parcel received successfully." : "Parcel have not arrived yet.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
    }
}
