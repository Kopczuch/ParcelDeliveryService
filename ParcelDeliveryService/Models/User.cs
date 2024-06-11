using ParcelDeliveryService.Core;


namespace ParcelDeliveryService.Models
{
    public class User
    {
        private object _userService;

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required Address Address { get; set; }

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