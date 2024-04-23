using ParcelDeliveryService.Core;

namespace ParcelDeliveryService.Models
{
    public class Parcel
    {
        public Parcel(string sender, string recipient, Size size, DateTime estimatedDeliveryTime, DateTime guaranteedDeliveryTime, int id = 0)
        {
            Id = id;
            Sender = sender;
            Recipient = recipient;
            Size = size;
            EstimatedDeliveryTime = estimatedDeliveryTime;
            GuaranteedDeliveryTime = guaranteedDeliveryTime;
            TransitHistory = new List<TransitEvent>();
        }

        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public Size Size { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime GuaranteedDeliveryTime { get; set; }
        public IList<TransitEvent> TransitHistory { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public DateTime? ActualPickUpTime { get; set; }
        public int? SenderLockerId { get; set; }
        public int? RecipientLockerId { get; set; }
        // Additional services

        public void Display()
        {
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Sender: {Sender}");
            Console.WriteLine($"Sender Locker Id: {SenderLockerId}");
            Console.WriteLine($"Recipient: {Recipient}");
            Console.WriteLine($"Recipient Locker Id: {RecipientLockerId}");
            Console.WriteLine($"Size: {Size}");
            Console.WriteLine($"Estimated Delivery Time: {EstimatedDeliveryTime}");
            Console.WriteLine($"Guaranteed Delivery Time: {GuaranteedDeliveryTime}");
        }
    }
}
