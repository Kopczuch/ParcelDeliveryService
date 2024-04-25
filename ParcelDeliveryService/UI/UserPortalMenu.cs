using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.UI
{
    public class UserPortalMenu : IMenu
    {
        private readonly IParcelService _parcelService;
        private readonly ILockerService _lockerService;

        public UserPortalMenu(
            IParcelService parcelService,
            ILockerService lockerService)
        {
            _parcelService = parcelService;
            _lockerService = lockerService;
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
                            RegisterParcel();
                            break;

                        case 2:
                            TrackParcel();
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
            Console.WriteLine("[1] Register Parcel");
            Console.WriteLine("[2] Track Parcel");
            Console.WriteLine("[0] Go Back");

            Console.WriteLine();
            Console.Write("Choose Operation: ");
            var operation = Console.ReadLine();

            return operation;
        }

        private void RegisterParcel()
        {
            Console.Clear();
            var parcel = new Parcel();

            Console.Write("Sender: ");
            parcel.Sender = Console.ReadLine();

            Console.WriteLine();

            Console.Write("Recipient: ");
            parcel.Recipient = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Size:");
            Console.WriteLine("[1] Small");
            Console.WriteLine("[2] Medium");
            Console.WriteLine("[3] Large");

            ChooseSize(parcel);

            var chosenLockerId = ChooseRecipientLocker();
            _lockerService.ReserveSlot(parcel, chosenLockerId);

            // TODO: Error handling
            parcel.RecipientLockerId = chosenLockerId;

            Console.WriteLine();
            Console.Write("Continue to payment? [y/n]: ");
            var decision = Console.ReadLine();

            // TODO: Infinite loop
            if (decision != "y") 
                return;

            PayForDelivery(parcel);

            Console.Clear();

            var registeredParcel = _parcelService.RegisterParcel(parcel);

            Console.Clear();
            Console.WriteLine("Parcel registry successful. Parcel info below:");
            Console.WriteLine();
            registeredParcel.Display();

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private void ChooseSize(Parcel parcel)
        {
            var choice = "";

            while (choice is not ("1" or "2" or "3"))
            {
                Console.WriteLine();
                Console.Write("Choice: ");

                choice = Console.ReadLine();

            }

            var size = int.Parse(choice);
            parcel.Size = size switch
            {
                1 => Size.Small,
                2 => Size.Medium,
                3 => Size.Large,
                _ => parcel.Size
            };
        }

        private int ChooseRecipientLocker()
        {
            Console.WriteLine();

            var availableLockers = _lockerService.GetVacantLockers();
            Console.WriteLine("Available recipient lockers:");
            foreach (var locker in availableLockers)
            {
                Console.WriteLine();
                locker.Display();
            }

            Console.WriteLine();
            Console.Write("Recipient LockerMenu Id: ");
            var chosenLockerId = Console.ReadLine();

            return int.Parse(chosenLockerId);
        }

        private void PayForDelivery(Parcel parcel)
        {
            Console.Clear();
            Console.WriteLine($"Price for the parcel: { parcel.Price }");

            Console.WriteLine("Press any key to pay...");
            Console.ReadLine();

            parcel.PaidFor = true;
        }

        private void TrackParcel()
        {
            Console.Clear();

            Console.Write("Provide parcel ID: ");

            // TODO: Error handling
            var id = int.Parse(Console.ReadLine());

            var parcel = _parcelService.GetParcel(id);
            
            Console.WriteLine();
            
            parcel.Display();

            Console.WriteLine();
            parcel.DisplayTransitHistory();

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
