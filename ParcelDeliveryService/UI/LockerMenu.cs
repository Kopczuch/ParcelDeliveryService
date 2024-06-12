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
                var requestedOperation = Menu();

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


                        case 0:
                            return;
                    }
                }
            }
        }

        private string? Menu()
        {
            Console.Clear();
            Console.WriteLine("[1] Deposit Parcel");
            Console.WriteLine("[2] Receive Parcel");
            Console.WriteLine("[3] Change locker address");

            Console.WriteLine("[0] Go Back");

            Console.WriteLine();
            Console.Write("Choose Operation: ");
            var operation = Console.ReadLine();

            return operation;
        }

        private void DepositParcel()
        {
            Console.Clear();
            Console.Write("Provide parcel ID: ");
            var parcelId = int.Parse(Console.ReadLine());

            var parcel = _parcelService.GetParcel(parcelId);
            
            if (parcel == null)
                throw new NullReferenceException();

            // TODO: State pattern
            //if (parcel.CurrentState != TransitEventType.Registered)
            //{
            //    Console.WriteLine($"Parcel #{ parcel.Id } was already deposited. Press any key to continue...");
            //    Console.ReadLine();
                
            //    return;
            //}

            var availableLockers = _lockerService.GetVacantLockers();

            foreach (var locker in availableLockers)
            {
                Console.WriteLine();
                locker.Display();
            }

            Console.WriteLine();
            Console.Write("Pass chosen locker ID: ");
            var chosenLockerId = int.Parse(Console.ReadLine());

            _lockerService.DepositParcel(parcel, chosenLockerId);
            
            parcel.SenderLockerId = chosenLockerId;
            parcel.AddDepositEvent();

            _parcelRepository.Update(parcel);

            Console.WriteLine();
            Console.WriteLine("Parcel deposit successful. Press any key to continue...");
            Console.ReadLine();
        }

        private void ReceiveParcel()
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

            var result = _lockerService.ReceiveFromLocker(parcelId, lockerId);
            _parcelService.PickUp(parcelId);

            Console.Clear();
            Console.WriteLine(result ? "Parcel received successfully." : "Parcel have not arrived yet.");

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private void ChangeAddress()
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
            var lockerId = int.Parse(Console.ReadLine());

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
    }
}
