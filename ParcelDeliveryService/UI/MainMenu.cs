using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.UI
{
    public class MainMenu : IMenu
    {
        private readonly IMenu _userPortalMenu;
        private readonly IMenu _lockerMenu;
        private readonly IMenu _transitMenu;

        public MainMenu(
            IMenu userPortalMenuMenu,
            IMenu lockerMenu,
            IMenu transitMenu)
        {
            _userPortalMenu = userPortalMenuMenu;
            _lockerMenu = lockerMenu;
            _transitMenu = transitMenu;
        }

        public void Run()
        {
            while (true)
            {
                var requestedOperation = Menu();

                if (int.TryParse(requestedOperation, out int operation))
                {
                    switch (operation)
                    {
                        case 1:
                            _userPortalMenu.Run();
                            break;

                        case 2:
                            _lockerMenu.Run();
                            break;

                        case 3:
                            _transitMenu.Run();
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

            Console.WriteLine("[1] User Portal");
            Console.WriteLine("[2] Lockers");
            Console.WriteLine("[3] Manage Parcel Transit");
            Console.WriteLine("[0] Exit");

            Console.Write("\nChoose operation: ");
            var operation = Console.ReadLine();

            return operation;
        }
    }
}
