using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Repositories;
using ParcelDeliveryService.Services;
using ParcelDeliveryService.UI;
using ParcelDeliveryService.Core;

namespace ParcelDeliveryService
{
    internal class Program
    {
        private readonly IMenu _mainMenu;
        private static Program? _instance;
        private static readonly object Lock = new();

        private Program(IMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }

        public static Program Instance(IMenu mainMenu)
        {
            lock (Lock)
            {
                if (_instance == null)
                {
                    _instance = new Program(mainMenu);
                }
                return _instance;
            }
        }

        public void Run()
        {
            _mainMenu.Run();
        }

        static void Main(string[] args)
        {
            // Dependencies
            var parcelRepository = new ParcelRepository();
            var lockerRepository = new LockerRepository();
            var userRepository = new UserRepository();

            var parcelService = new ParcelService(parcelRepository);
            var userService = new UserService(userRepository);
            var lockerService = new LockerService(lockerRepository);
            var rerouteService = new RerouteServiceDecorator(parcelService);

            var userPortalMenu = new UserPortalMenu(parcelService, lockerService, lockerRepository, rerouteService, userService);
            var lockerMenu = new LockerMenu(lockerService, parcelService);
            var transitMenu = new TransitMenu(parcelService, lockerService);

            var mainMenu = new MainMenu(userPortalMenu, lockerMenu, transitMenu);

            var program = Instance(mainMenu);

            program.Run();
        }
    }
}
