namespace ParcelDeliveryService.Models
{
    public class LockerHistoryItem
    {
        public int SlotId { get; set; }
        public int ParcelId { get; set; }
        public DateTime? Deposited { get; set; }
        public DateTime? Received { get; set; }

        public void Display()
        {
            Console.WriteLine($"Parcel #{ParcelId}");
            Console.WriteLine($"Slot #{SlotId}");
            Console.WriteLine($"Deposited: {Deposited:d}");
            Console.WriteLine($"Received: {Received:d}");
        }
    }
}
