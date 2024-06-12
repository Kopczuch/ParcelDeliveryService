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

        public Parcel(int senderId, int recipientId, Size size, int recipientLockerId)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            Size = size;
            RecipientLockerId = recipientLockerId;
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
        private ParcelState State { get; set; }
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
            Console.Write("Sender: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{SenderId}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Sender LockerMenu Id: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{SenderLockerId}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Recipient: ");
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
