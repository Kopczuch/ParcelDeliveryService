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
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Parcel # ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ParcelId);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Slot # ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(SlotId);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Deposited: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Deposited);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Received: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Received);
            Console.ResetColor();
            Console.WriteLine("-------------------------------");
        }
    }
}
