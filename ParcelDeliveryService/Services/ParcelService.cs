using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using System.Diagnostics;

namespace ParcelDeliveryService.Services
{
    public class ParcelService : IParcelService
    {
        private IParcelRepository _parcelRepository;

        public ParcelService(IParcelRepository parcelRepository)
        {
            _parcelRepository = parcelRepository;
        }


        public Parcel RegisterParcel(Parcel parcel)
        {
            _parcelRepository.Add(parcel);

            return parcel;
        }

        public void DepositParcel(int parcelId, int senderLockerId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            parcel.SenderLockerId = senderLockerId;
            parcel.AddDepositEvent();
            _parcelRepository.Update(parcel);
        }

        public IList<Parcel> ListParcels()
        {
            return (IList<Parcel>)_parcelRepository.GetAll();
        }

        public Parcel? GetParcel(int parcelId)
        {
            return _parcelRepository.GetById(parcelId);
        }

        

        public void ForwardInTransit(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            switch (parcel.CurrentState)
            {
                case TransitEventType.Deposited:
                    parcel.AddReceivedFromSenderLockerEvent();
                    break;

                case TransitEventType.ReceivedFromSenderLocker:
                    parcel.AddInStorageEvent();
                    break;

                case TransitEventType.InStorage:
                    parcel.AddInTransitEvent();
                    break;

                case TransitEventType.InTransit:
                    parcel.AddReadyForPickUpEvent();
                    break;

                case TransitEventType.ReadyForPickUp:
                    parcel.AddDeadlineOverEvent();
                    break;

                case TransitEventType.DeadlineOver:
                    parcel.AddInExternalStorageEvent();
                    break;

                case TransitEventType.InExternalStorage:
                    parcel.AddDestroyedEvent();
                    break;
            }

            _parcelRepository.Update(parcel);
        }

        public void PickUp(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            parcel.AddPickUpEvent();
            _parcelRepository.Update(parcel);
        }


        /*private void EstimateDeliveryTime(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var random = new Random();

            if (parcel.Size == Size.Small)
                parcel.EstimatedDeliveryTime = DateTime.Today.AddDays(random.Next(1, 5));

            if (parcel.Size == Size.Medium)
                parcel.EstimatedDeliveryTime = DateTime.Today.AddDays(random.Next(3, 8));

            if (parcel.Size == Size.Large)
                parcel.EstimatedDeliveryTime = DateTime.Today.AddDays(random.Next(7, 15));

            _parcelRepository.Update(parcel);
        }

        private void AssignGuaranteedDeliveryTime(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            if (parcel.Size == Size.Small)
                parcel.GuaranteedDeliveryTime = DateTime.Today.AddDays(4);

            if (parcel.Size == Size.Medium)
                parcel.GuaranteedDeliveryTime = DateTime.Today.AddDays(7);

            if (parcel.Size == Size.Large)
                parcel.GuaranteedDeliveryTime = DateTime.Today.AddDays(14);

            _parcelRepository.Update(parcel);
        }

        private void CalculateCost(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            if (parcel.Size == Size.Small)
                parcel.Price = 20;

            if (parcel.Size == Size.Medium)
                parcel.Price = 35;

            if (parcel.Size == Size.Large)
                parcel.Price = 50;

            _parcelRepository.Update(parcel);
        }

        public void AddRegistryEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Sender",
                Type = TransitEventType.Registered
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddDepositEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{parcel.SenderLockerId}",
                Type = TransitEventType.Deposited
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddReceivedFromSenderLockerEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Courier",
                Type = TransitEventType.ReceivedFromSenderLocker
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddInStorageEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In Storage",
                Type = TransitEventType.InStorage
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddInTransitEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In Transit",
                Type = TransitEventType.InTransit
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddReadyForPickUpEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{parcel.RecipientLockerId}",
                Type = TransitEventType.ReadyForPickUp
            };

            parcel.ActualDeliveryTime = DateTime.Now;
            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddDeadlineOverEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{parcel.RecipientLockerId}",
                Type = TransitEventType.DeadlineOver
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddInExternalStorageEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In External Storage",
                Type = TransitEventType.InExternalStorage
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddDestroyedEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In External Storage",
                Type = TransitEventType.Destroyed
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }

        public void AddPickUpEvent(int parcelId)
        {
            var parcel = _parcelRepository.GetById(parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Recipient",
                Type = TransitEventType.PickedUp
            };

            parcel.TransitHistory.Add(transitEvent);
            _parcelRepository.Update(parcel);
        }*/
    }
}
