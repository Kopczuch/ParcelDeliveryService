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

        public Parcel(string sender, string recipient, Size size, int recipientLockerId)
        {
            Sender = sender;
            Recipient = recipient;
            Size = size;
            RecipientLockerId = recipientLockerId;
            TransitHistory = new List<TransitEvent>();
            State = new RegisteredState();
        }

        public Parcel(int id, string sender, string recipient, Size size, int recipientLockerId)
        {
            Id = id;
            Sender = sender;
            Recipient = recipient;
            Size = size;
            RecipientLockerId = recipientLockerId;
            TransitHistory = new List<TransitEvent>();
            State = new RegisteredState();
        }


        public int Id { get; set; }
        public ParcelState State { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
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
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Sender: {Sender}");
            Console.WriteLine($"Sender LockerMenu Id: {SenderLockerId}");
            Console.WriteLine($"Recipient: {Recipient}");
            Console.WriteLine($"Recipient LockerMenu Id: {RecipientLockerId}");
            Console.WriteLine($"Size: {Size}");
            Console.WriteLine($"Estimated Delivery Time: {EstimatedDeliveryTime:d}");
            Console.WriteLine($"Guaranteed Delivery Time: {GuaranteedDeliveryTime:d}");
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

        public void Lose(IParcelService parcelService)
        {
            State.Lose(this, parcelService);
        }

        public void Destroy(IParcelService parcelService)
        {
            State.Destroy(this, parcelService);
        }

        public bool IsTransitFinished()
        {
            return State.IsTerminalState;
        }
    }
}
