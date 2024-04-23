namespace ParcelDeliveryService.Models
{
    public class Address
    {
        public string Country { get; set; } = "Poland";
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
    }
}
