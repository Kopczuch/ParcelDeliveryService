using ParcelDeliveryService.Core;


namespace ParcelDeliveryService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; }

        public User(int id, string name, string surname, string phoneNumber, string email, string password, Address address)
        {
            Id = id;
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            Address = address;
        }

        public void Display()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Id}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Name: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Name}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Surname: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Surname}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("PhoneNumber: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{PhoneNumber}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Email: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Email}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Address: ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Address.Country}, {Address.City}, {Address.PostalCode}, {Address.Street} {Address.StreetNumber}");
            Console.ResetColor();
            Console.WriteLine("-------------------------------------------------");
        }
    }

    
}