using ParcelDeliveryService.Core;
using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Models
{
    public class Parcel
    {
        public Parcel()
        {
            TransitHistory = new List<TransitEvent>();
            State = new RegisteredState();
        }

        public Parcel(int id, int senderId, int recipientId, Size size, int recipientLockerId)
        {
            Id = id;
            SenderId = senderId;
            RecipientId = recipientId;
            Size = size;
            RecipientLockerId = recipientLockerId;
            TransitHistory = new List<TransitEvent>();
            State = new RegisteredState();
        }


        public int Id { get; set; }
        public ParcelState State { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime GuaranteedDeliveryTime { get; set; }
        public IList<TransitEvent> TransitHistory { get; set; }
        public TransitEventType CurrentState => TransitHistory.Last().Type;
        public DateTime? ActualDeliveryTime { get; set; }
        public DateTime? ActualPickUpTime { get; set; }
        public int? SenderLockerId { get; set; }
        public int RecipientLockerId { get; set; }
        public double Price { get; set; }
        public bool PaidFor { get; set; } = false;
        public double AdditionalCosts { get; set; } = 0.0;
        public bool CanExtend { get; set; } = true;

        private Size _size;
        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                EstimateDeliveryTime();
                AssignGuaranteedDeliveryTime();
                CalculateCost();
            }
        }


        // Additional services
        public void Display()
        {
            Console.WriteLine("Parcel Information");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Id}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Sender Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{SenderId}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Sender LockerMenu Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{SenderLockerId}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Recipient Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{RecipientId}");


            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Recipient LockerMenu Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{RecipientLockerId}");
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Size: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{Size}");
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Estimated Delivery Time: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{EstimatedDeliveryTime:d}");
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Guaranteed Delivery Time: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{GuaranteedDeliveryTime:d}");
            Console.ResetColor();
            Console.WriteLine("-------------------------------------------------");
        }

        public void DisplayTransitHistory()
        {
            Console.WriteLine("Transit History");

            foreach (var transitEvent in TransitHistory)
            {
                Console.WriteLine();
                Console.WriteLine($"Status: {transitEvent.Type}");
                Console.WriteLine($"Location: {transitEvent.Location}");
                Console.WriteLine($"Timestamp: {transitEvent.TimeStamp}");
            }
        }

        public void CalculateAdditionalFee()
        {
            AdditionalCosts = Price / 5;
        }

        private void EstimateDeliveryTime()
        {
            var random = new Random();

            if (Size == Size.Small)
                EstimatedDeliveryTime = DateTime.Today.AddDays(random.Next(1, 5));

            if (Size == Size.Medium)
                EstimatedDeliveryTime = DateTime.Today.AddDays(random.Next(3, 8));

            if (Size == Size.Large)
                EstimatedDeliveryTime = DateTime.Today.AddDays(random.Next(7, 15));
        }

        private void AssignGuaranteedDeliveryTime()
        {
            if (Size == Size.Small)
                GuaranteedDeliveryTime = DateTime.Today.AddDays(4);

            if (Size == Size.Medium)
                GuaranteedDeliveryTime = DateTime.Today.AddDays(7);

            if (Size == Size.Large)
                GuaranteedDeliveryTime = DateTime.Today.AddDays(14);
        }

        private void CalculateCost()
        {
            if (Size == Size.Small)
                Price = 20;

            if (Size == Size.Medium)
                Price = 35;

            if (Size == Size.Large)
                Price = 50;
        }

        public void ForwardInTransit(IParcelService parcelService, ILockerService lockerService)
        {
            State.HandleForwardInTransit(this, parcelService, lockerService);
        }

        public void Lose(IParcelService parcelService, ILockerService lockerService)
        {
            State.Lose(this, parcelService, lockerService);
        }

        public void Destroy(IParcelService parcelService, ILockerService lockerService)
        {
            State.Destroy(this, parcelService, lockerService);
        }

        public bool IsTransitFinished()
        {
            return State.IsTerminalState;
        }
    }
}
