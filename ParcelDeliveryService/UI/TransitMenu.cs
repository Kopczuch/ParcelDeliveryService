using ParcelDeliveryService.Interfaces;

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
            while (true)
            {
                Console.Clear();
                var parcel = _parcelService.GetParcel(parcelId);

                if (parcel == null)
                    return;

                parcel.Display();
                Console.WriteLine();
                parcel.DisplayTransitHistory();
                Console.WriteLine();

                if (parcel.IsTransitFinished())
                {
                    Console.WriteLine("Parcel transit is finished. Press any key to continue...");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("Operations:");
                Console.WriteLine("[1] Forward In Transit");
                Console.WriteLine("[2] Lose parcel");
                Console.WriteLine("[3] Destroy parcel");
                Console.WriteLine("[0] Go Back");

                Console.WriteLine();
                Console.Write("Execute operation: ");
                var choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        parcel.ForwardInTransit(_lockerService);
                        break;

                    case 2:
                        parcel.Lose();
                        break;

                    case 3:
                        parcel.Destroy();
                        break;

                    case 0:
                        return;
                }
            }
        }
    }
}
