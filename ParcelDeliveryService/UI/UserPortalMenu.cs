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
                        case 7:
                            ExpandLocker();
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
            Console.WriteLine("[3] Register User");
            Console.WriteLine("[4] Login User");
            Console.WriteLine("[5] Show Users");
            Console.WriteLine("[6] Reroute Parcel"); // New menu option for rerouting a parcel
            Console.WriteLine("[7] Expand Locker"); 
            Console.WriteLine("[0] Go Back");

            Console.WriteLine();
            Console.Write("Choose Operation: ");
            var operation = Console.ReadLine();

            return operation;
        }

        private void ExpandLocker()
        {
            Console.Clear();
            Console.WriteLine("Select a locker to expand:");

            var lockers = _lockerService.GetLockers();

            foreach (var locker in lockers)
            {
                Console.WriteLine($"[{locker.Id}] Locker #{locker.Id}");
            }

            Console.Write("Enter locker ID: ");
            var lockerId = int.Parse(Console.ReadLine());

            var selectedLocker = lockers.FirstOrDefault(l => l.Id == lockerId);

            if (selectedLocker == null)
            {
                Console.WriteLine("Invalid locker ID.");
                return;
            }

            Console.Write("Enter the number of slots to add: ");
            var slotsToAdd = int.Parse(Console.ReadLine());

            var compositeLocker = new LockerComposite(selectedLocker);

            for (int i = 0; i < slotsToAdd; i++)
            {
                Console.WriteLine($"Slot {i + 1}:");
                Console.Write("Enter slot size (Small, Medium, Large): ");
                var size = Enum.Parse<Size>(Console.ReadLine());
                compositeLocker.AddSlot(new Slot { Size = size, Vacancy = VacancyState.Vacant });
            }

            // Update locker with additional slots
            foreach (var slot in compositeLocker.AdditionalSlots)
            {
                selectedLocker.Slots.Add(slot);
            }

            // Update locker in repository
            _lockerRepository.Update(selectedLocker);

            Console.WriteLine($"Locker #{lockerId} expanded successfully.");
        }

        private void RegisterParcel()
        {
            Console.Clear();
            var parcel = new Parcel();

            User sender = _userService.GetCurrentUser();
            if( sender == null )
            {
                Console.WriteLine("You must sign in before registering a parcel."); 
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            parcel.SenderId = sender.Id;

            Console.WriteLine();
            ShowUsers();

            User receipient;
            while (true)
            {
                Console.Write("Recipient UserId: ");
                int receipientId = int.Parse(Console.ReadLine());
                receipient = _userService.GetUser(receipientId);
                if( receipient != null)
                {
                    parcel.RecipientId = receipientId;
                    break;
                }
                Console.Write("Invalid Id, please try again!");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            Console.Clear();
            Console.Write("Sender: ");
            Console.WriteLine($"{sender.Name} {sender.Surname}");
            Console.Write("Receipient: ");
            Console.WriteLine($"{receipient.Name} {receipient.Surname}");

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

        public void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("Enter the following details to create a new user:");

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Surname: ");
            string surname = Console.ReadLine();

            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Enter Address Details:");
            Console.Write("Country: ");
            string country = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            Console.Write("Zip Code: ");
            string zipCode = Console.ReadLine();

            Console.Write("Street: ");
            string street = Console.ReadLine();

            Console.Write("Apartment number: ");
            string apartment = Console.ReadLine();


            Address address = new Address(country, city, zipCode, street, apartment);
            bool isEmailValid = false;
            bool firstTry = true;

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

                User newUser = new User(0, name, surname, phoneNumber, email, password, address);
                isEmailValid = _userService.RegisterUser(newUser);

                if (isEmailValid)
                {
                    Console.WriteLine("User created successfully. Press any key to continue...");
                }
                else
                {
                    Console.WriteLine($"The {email} email address is already used.");
                    Console.WriteLine("Do you want to enter a different email address and continue the registration process? [y/n]");

                    string response = Console.ReadLine().ToLower();
                    if (response != "y" && response != "yes")
                    {
                        Console.WriteLine("Registration process stopped. Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            Console.ReadLine();
        }

        private void LoginUser()
        {
            Console.Clear();
            Console.WriteLine("Enter your credentials to sign in:");

            bool authorized = false;

            while(!authorized)
            {
                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                authorized = _userService.LoginUser(email, password);

                if (authorized)
                {
                    Console.WriteLine("Success! You are now logged in. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Credentials are incorrect.");
                Console.WriteLine("Do you want to try again? [y/n]");

                string response = Console.ReadLine().ToLower();
                if (response != "y" && response != "yes")
                {
                    return;
                }
            }
        }

        private void ShowUsers()
        {
            Console.Clear();

            var users = _userService.ListUsers();
            Console.WriteLine("Registered users:");
            foreach (var user in users)
            {
                Console.WriteLine();
                user.Display();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void RerouteParcel()
        {
        
            Console.Clear();

            Console.Write("Provide parcel ID: ");
            var parcelId = int.Parse(Console.ReadLine());

            Console.Write("Provide new locker ID: ");
            var newLockerId = int.Parse(Console.ReadLine());

            var parcel = _parcelService.GetParcel(parcelId);

            _lockerService.ReserveSlot(parcel, newLockerId);
            _lockerService.ReleaseSlot(parcel, parcel.RecipientLockerId);

            _rerouteService.Reroute(parcel, newLockerId);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

    }
}
