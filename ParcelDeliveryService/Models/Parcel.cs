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

        public int Id { get; set; }
        private ParcelState State { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }

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

        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime GuaranteedDeliveryTime { get; set; }
        public IList<TransitEvent> TransitHistory { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public DateTime? ActualPickUpTime { get; set; }
        public int? SenderLockerId { get; set; }
        public int RecipientLockerId { get; set; }
        public double Price { get; set; }
        public bool PaidFor { get; set; } = false;
        public double AdditionalCosts { get; set; } = 0.0;
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

        public void ForwardInTransit(ILockerService lockerService)
        {
            State.HandleForwardInTransit(this, lockerService);
        }

        public void Lose()
        {
            State.Lose(this);
        }

        public void Destroy()
        {
            State.Destroy(this);
        }

        public bool IsTransitFinished()
        {
            return State.IsTerminalState;
        }

        public void AddRegistryEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Sender",
                Type = TransitEventType.Registered
            };

            TransitHistory.Add(transitEvent);
        }

        public void AddDepositEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{SenderLockerId}",
                Type = TransitEventType.Deposited
            };

            State = new DepositedState();

            TransitHistory.Add(transitEvent);
        }

        public void AddReceivedFromSenderLockerEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Courier",
                Type = TransitEventType.ReceivedFromSenderLocker
            };

            State = new ReceivedFromSenderLockerState();

            TransitHistory.Add(transitEvent);
        }

        public void AddInStorageEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In Storage",
                Type = TransitEventType.InStorage
            };

            State = new InStorageState();

            TransitHistory.Add(transitEvent);
        }

        public void AddInTransitEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In Transit",
                Type = TransitEventType.InTransit
            };

            State = new InTransitState();

            TransitHistory.Add(transitEvent);
        }

        public void AddReadyForPickUpEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{RecipientLockerId}",
                Type = TransitEventType.ReadyForPickUp
            };

            ActualDeliveryTime = DateTime.Now;

            State = new ReadyForPickupState();

            TransitHistory.Add(transitEvent);
        }

        public void AddDeadlineOverEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{RecipientLockerId}",
                Type = TransitEventType.DeadlineOver
            };

            State = new DeadlineOverState();

            TransitHistory.Add(transitEvent);
        }

        public void AddInExternalStorageEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In External Storage",
                Type = TransitEventType.InExternalStorage
            };

            State = new InExternalStorageState();

            TransitHistory.Add(transitEvent);
        }

        public void AddLostEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "Unknown",
                Type = TransitEventType.Lost
            };

            State = new LostState();

            TransitHistory.Add(transitEvent);
        }

        public void AddDestroyedEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In External Storage",
                Type = TransitEventType.Destroyed
            };

            State = new DestroyedState();

            TransitHistory.Add(transitEvent);
        }

        public void AddPickUpEvent()
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Recipient",
                Type = TransitEventType.PickedUp
            };

            State = new PickedUpState();

            TransitHistory.Add(transitEvent);
        }
    }
}
