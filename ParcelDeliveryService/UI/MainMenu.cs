using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.UI
{
    public class MainMenu : IMenu
    {
        private readonly IMenu _userPortalMenu;
        private readonly IMenu _lockerMenu;
        private readonly IMenu _transitMenu;

        public MainMenu(
            IMenu userPortalMenu,
            IMenu lockerMenu,
            IMenu transitMenu)
        {
            _userPortalMenu = userPortalMenu;
            _lockerMenu = lockerMenu;
            _transitMenu = transitMenu;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    var requestedOperation = DisplayMenu();

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

                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid option. Please try again.");
                                Console.ResetColor();
                                break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please enter a number.");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ResetColor();
                }
                finally
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        private string? DisplayMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[1] User Portal");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[2] Lockers");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[3] Manage Parcel Transit");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[0] Exit");
            Console.ResetColor();

            Console.Write("\nChoose operation: ");
            return Console.ReadLine();
        }
    }
}
