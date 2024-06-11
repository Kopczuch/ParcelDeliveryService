using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.UI
{
    public class TransitMenu : IMenu
    {
        private readonly IParcelService _parcelService;
        private readonly ILockerService _lockerService;

        public TransitMenu(
            IParcelService parcelService,
            ILockerService lockerService)
        {
            _parcelService = parcelService;
            _lockerService = lockerService;
        }

        public void Run()
        {
            Console.Clear();
            Console.WriteLine("Please choose parcel for management.");
            DisplayParcels();

            Console.WriteLine();
            Console.Write("Pass parcel ID: ");
            var parcelId = int.Parse(Console.ReadLine());

            ManageParcelTransit(parcelId);
        }

        private void DisplayParcels()
        {
            var parcels = _parcelService.ListParcels();

            foreach (var parcel in parcels)
            {
                Console.WriteLine();
                parcel.Display();
            }
        }

        private void ManageParcelTransit(int parcelId)
        {
            var choice = -1;

            while (choice != 0)
            {
                Console.Clear();
                var parcel = _parcelService.GetParcel(parcelId);

                if (parcel == null)
                    return;

                parcel.Display();
                Console.WriteLine();
                parcel.DisplayTransitHistory();
                Console.WriteLine();

                var currentState = parcel.CurrentState;

                if (currentState == TransitEventType.Registered)
                {
                    Console.WriteLine("Parcel has not been deposited yet.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                if (currentState == TransitEventType.PickedUp)
                {
                    Console.WriteLine("Parcel has been delivered.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Operations:");
                Console.WriteLine("[1] Forward In Transit");

                if (currentState == TransitEventType.ReceivedFromSenderLocker ||
                    currentState == TransitEventType.InStorage ||
                    currentState == TransitEventType.InTransit)
                    Console.WriteLine("[2] Lose");

                Console.WriteLine("[0] Go Back");

                Console.WriteLine();
                Console.Write("Execute operation: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        ForwardInTransit(parcel);
                        break;

                    case 2:
                        break;

                    case 0:
                        return;
                }
            }
        }

        private void ForwardInTransit(Parcel parcel)
        {
            if (parcel.CurrentState == TransitEventType.Deposited)
                _lockerService.ReceiveFromLocker(parcel.Id, parcel.SenderLockerId!.Value);

            _parcelService.ForwardInTransit(parcel);

            if (parcel.CurrentState == TransitEventType.ReadyForPickUp)
                _lockerService.DepositParcel(parcel, parcel.RecipientLockerId);

        }
    }
}
