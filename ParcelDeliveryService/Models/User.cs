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
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Surname: {Surname}");
            Console.WriteLine($"PhoneNumber: {PhoneNumber}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Address: {Address.Country}, {Address.City}, {Address.PostalCode}, {Address.Street} {Address.StreetNumber}");
        }
    }

    
}