using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Services;

namespace ParcelDeliveryService
{
    internal class Program
    {
        private readonly IParcelService _parcelService;

        public Program(
            IParcelService parcelService)
        {
            _parcelService = parcelService;
        }

        public void Run()
        {
            Console.WriteLine("Hello!");

            Console.WriteLine("1 - Register Package");
            Console.WriteLine("2 - Deposit Package");
            Console.WriteLine("3 - List Packages");
            Console.WriteLine("4 - Get Package");

            Console.Write("\nChoice: ");
            var requestedOperation = Console.ReadLine();

            if (int.TryParse(requestedOperation, out int operation))
            {
                switch (operation)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        var parcels = _parcelService.ListParcels();
                        foreach (var parcel in parcels)
                        {
                            Console.WriteLine("");
                            parcel.Display();
                        }
                        break;
                    case 4:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
            
        }

        static void Main(string[] args)
        {
            var program = new Program(new ParcelService());

            program.Run();
        }
    }
}
