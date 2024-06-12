using ParcelDeliveryService.Interfaces;
using System;

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
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Please choose a parcel for management.");
                    DisplayParcels();

                    Console.WriteLine();
                    Console.Write("Enter parcel ID (or 'X' to exit): ");
                    var input = Console.ReadLine();
                    if (input?.ToUpper() == "X")
                    {
                        break;
                    }
                    else if (int.TryParse(input, out var parcelId))
                    {
                        ManageParcelTransit(parcelId);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid parcel ID. Please enter a valid number.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        private void DisplayParcels()
        {
            try
            {
                var parcels = _parcelService.ListParcels();
                foreach (var parcel in parcels)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    parcel.Display();
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while listing parcels: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private void ManageParcelTransit(int parcelId)
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    var parcel = _parcelService.GetParcel(parcelId);

                    if (parcel == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Parcel not found. Returning to the previous menu.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        return;
                    }

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    parcel.Display();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    parcel.DisplayTransitHistory();
                    Console.WriteLine();

                    if (parcel.IsTransitFinished())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Parcel transit is finished. Press any key to continue...");
                        Console.ForegroundColor = ConsoleColor.Gray;
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
                    if (int.TryParse(Console.ReadLine(), out var choice))
                    {
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

                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid choice. Please select a valid operation.");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please enter a number.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred while managing the parcel: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                
            }
        }
    }
}
