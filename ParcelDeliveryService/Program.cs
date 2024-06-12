using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Services;
using ParcelDeliveryService.UI;
using ParcelDeliveryService.Core;

namespace ParcelDeliveryService
{
    internal class Program
    {
        private readonly IMenu _mainMenu;

        public Program(IMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }

        public void Run()
        {
            _mainMenu.Run();
        }

        static void Main(string[] args)
        {
            // Dependencies
            var parcelService = new ParcelService();
            var lockerService = new LockerService();
            var rerouteService = new RerouteServiceDecorator(parcelService);
            
            var userPortalMenu = new UserPortalMenu(parcelService, lockerService, rerouteService);
            var lockerMenu = new LockerMenu(lockerService, parcelService);
            var transitMenu = new TransitMenu(parcelService, lockerService);

            var program = new Program(
                new MainMenu(
                    userPortalMenu,
                    lockerMenu,
                    transitMenu
                )
            );

            program.Run();
        }
    }
}
