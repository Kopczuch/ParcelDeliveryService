namespace ParcelDeliveryService.Models
{
    public class Address
    {
        public string Country { get; set; } = "Poland";
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

        public Address(string country, string city, string postalCode, string street, string streetNumber)
        {
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
            StreetNumber = streetNumber;
        }
    }
}
