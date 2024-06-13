using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.UI
{
    public class UserPortalMenu : IMenu
    {
        private readonly IParcelService _parcelService;
        private readonly ILockerService _lockerService;
        private readonly ILockerRepository _lockerRepository;
        private readonly IUserService _userService;
        private readonly IRerouteService _rerouteService;

        public UserPortalMenu(
            IParcelService parcelService,
            ILockerService lockerService,
            ILockerRepository lockerRepository,
            IRerouteService rerouteService,
            IUserService userService)
        {
            _parcelService = parcelService;
            _lockerService = lockerService;
            _lockerRepository = lockerRepository;
            _rerouteService = rerouteService;
            _userService = userService;
        }

        public void Run()
        {
            while (true)
            {
                try
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

                            case 3:
                                RegisterUser();
                                break;

                            case 4:
                                LoginUser();
                                break;

                            case 5:
                                ShowUsers();
                                break;

                            case 6:
                                RerouteParcel();
                                break;

                            case 0:
                                return;

                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid option. Please choose a valid operation. Press any key to continue...");
                                Console.ResetColor();
                                Console.ReadLine();
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred: {ex.Message} Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                }
                
            }
        }

        private string? Menu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[1] Register Parcel");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[2] Track Parcel");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[3] Register User");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[4] Login User");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[5] Show Users");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[6] Reroute Parcel");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[0] Go Back");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Choose Operation: ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        private void RegisterParcel()
        {
            Console.Clear();
            try
            {
                var parcel = new Parcel();
                var sender = _userService.GetCurrentUser();
                if (sender == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must sign in before registering a parcel. Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    return;
                }

                parcel.SenderId = sender.Id;

                Console.WriteLine();
                ShowUsers();

                User recipient = null;
                while (recipient == null)
                {
                    Console.Write("Recipient UserId: ");
                    if (int.TryParse(Console.ReadLine(), out var recipientId))
                    {
                        recipient = _userService.GetUser(recipientId);
                        if (recipient != null)
                        {
                            parcel.RecipientId = recipientId;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Id, please try again!");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        Console.ResetColor();
                    }
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Sender: {sender.Name} {sender.Surname}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Recipient: {recipient.Name} {recipient.Surname}");
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("Size:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[1] Small");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[2] Medium");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("[3] Large");
                Console.ResetColor();
                ChooseSize(parcel);

                var chosenReceipientLockerId = ChooseReceipientLocker();

                Console.WriteLine();
                Console.Write("Continue to payment? [y/n]: ");
                var decision = Console.ReadLine()?.ToLower();

                if (decision != "y")
                    return;

                _lockerService.ReserveSlot(parcel, chosenReceipientLockerId);
                parcel.RecipientLockerId = chosenReceipientLockerId;

                PayForDelivery(parcel);

                var registeredParcel = _parcelService.RegisterParcel(parcel);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Parcel registry successful. Parcel info below:");
                Console.ResetColor();
                registeredParcel.Display();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while registering the parcel: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void ChooseSize(Parcel parcel)
        {
            var validInput = false;
            while (!validInput)
            {
                Console.Write("Choice: ");
                var choice = Console.ReadLine();

                if (int.TryParse(choice, out var size) && size is >= 1 and <= 3)
                {
                    parcel.Size = size switch
                    {
                        1 => Size.Small,
                        2 => Size.Medium,
                        3 => Size.Large,
                        _ => parcel.Size
                    };
                    validInput = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    Console.ResetColor();
                }
            }
        }

        private int ChooseReceipientLocker()
        {
            while (true)
            {
                var availableLockers = _lockerService.GetVacantLockers();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Available recipient lockers:");
                foreach (var locker in availableLockers)
                {
                    locker.Display();
                }
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Chosen Receipient Locker Id: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out var chosenLockerId))
                {
                    return chosenLockerId;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid locker ID. Please enter a valid number.");
                Console.ResetColor();
            }
        }

        private void PayForDelivery(Parcel parcel)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Price for the parcel: ${parcel.Price}");
            Console.ResetColor();
            Console.WriteLine("Press any key to pay...");
            Console.ReadKey();
            parcel.PaidFor = true;
        }

        private void TrackParcel()
        {
            Console.Clear();
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Provide parcel ID: ");
                Console.ResetColor();
                if (!int.TryParse(Console.ReadLine(), out var id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid parcel ID. Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    return;
                }

                var parcel = _parcelService.GetParcel(id);
                if (parcel == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Parcel not found. Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadLine();
                    return;
                }

                parcel.Display();
                parcel.DisplayTransitHistory();

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while tracking the parcel: {ex.Message} Press any key to continue...");
                Console.ResetColor();
                Console.ReadLine();
            }
        }

        public void RegisterUser()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Enter the following details to create a new user:");

                Console.Write("Name: ");
                var name = Console.ReadLine();

                Console.Write("Surname: ");
                var surname = Console.ReadLine();

                Console.Write("Phone Number: ");
                var phoneNumber = Console.ReadLine();

                Console.Write("Email: ");
                var email = Console.ReadLine();

                Console.Write("Password: ");
                var password = Console.ReadLine();

                Console.WriteLine("Enter Address Details:");
                Console.Write("Country: ");
                var country = Console.ReadLine();

                Console.Write("City: ");
                var city = Console.ReadLine();

                Console.Write("Zip Code: ");
                var zipCode = Console.ReadLine();

                Console.Write("Street: ");
                var street = Console.ReadLine();

                Console.Write("Apartment number: ");
                var apartment = Console.ReadLine();

                var address = new Address(country, city, zipCode, street, apartment);
                var isEmailValid = false;
                var firstTry = true;

                while (!isEmailValid)
                {
                    if (!firstTry)
                    {
                        Console.Write("Email: ");
                        email = Console.ReadLine();
                    }
                    else
                    {
                        firstTry = false;
                    }

                    var newUser = new User(0, name, surname, phoneNumber, email, password, address);
                    isEmailValid = _userService.RegisterUser(newUser);

                    if (isEmailValid)
                    {
                        Console.WriteLine("User created successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"The {email} email address is already used.");
                        Console.WriteLine("Do you want to enter a different email address and continue the registration process? [y/n]");

                        var response = Console.ReadLine().ToLower();
                        if (response != "y" && response != "yes")
                        {
                            Console.WriteLine("Registration process stopped.");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while registering the user: {ex.Message}");
            }
        }

        private void LoginUser()
        {
            Console.Clear();
            try
            {
                Console.ResetColor();
                Console.WriteLine("Enter your credentials to sign in:");
                var authorized = false;

                while (!authorized)
                {
                    Console.Write("Email: ");
                    var email = Console.ReadLine();

                    Console.Write("Password: ");
                    var password = Console.ReadLine();

                    authorized = _userService.LoginUser(email, password);

                    if (authorized)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Success! You are now logged in. Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("Credentials are incorrect. Do you want to try again? [y/n]");
                    var response = Console.ReadLine().ToLower();
                    if (response != "y" && response != "yes")
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging in: {ex.Message}");
            }
        }

        private void ShowUsers()
        {
            Console.Clear();
            try
            {
                var users = _userService.ListUsers();
                Console.WriteLine("Registered users:");
                foreach (var user in users)
                {
                    user.Display();
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPress any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private void RerouteParcel()
        {
            Console.Clear();
            try
            {
                Console.Write("Provide parcel ID: ");
                if (!int.TryParse(Console.ReadLine(), out var parcelId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid parcel ID.");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Provide new locker ID: ");
                if (!int.TryParse(Console.ReadLine(), out var newLockerId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid locker ID.");
                    Console.ResetColor();
                    return;
                }

                var parcel = _parcelService.GetParcel(parcelId);
                if (parcel == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Parcel not found.");
                    Console.ResetColor();
                    return;
                }

                _lockerService.ReserveSlot(parcel, newLockerId);
                _lockerService.ReleaseSlot(parcel, parcel.RecipientLockerId);
                _rerouteService.Reroute(parcel, newLockerId);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Parcel rerouted successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while rerouting the parcel: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}