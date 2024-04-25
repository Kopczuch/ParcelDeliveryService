﻿using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Services
{
    public class ParcelService : IParcelService
    {
        private readonly IList<Parcel> _parcels;

        public ParcelService()
        {
            _parcels = new List<Parcel>
            {
                //new Parcel("James", "Edward", Size.Small, DateTime.Now.AddDays(4), DateTime.Now.AddDays(7), 1),
                //new Parcel("Samantha", "Andrew", Size.Large, DateTime.Now.AddDays(10), DateTime.Now.AddDays(14), 2),
                //new Parcel("Andrew", "James", Size.ExtraSmall, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), 3),
            };
        }

        public Parcel RegisterParcel(Parcel parcel)
        {
            parcel.Id = _parcels.Count + 1;
            parcel.AddRegistryEvent();
            _parcels.Add(parcel);

            return parcel;
        }

        public void DepositParcel(int parcelId, int senderLockerId)
        {
            var parcel = _parcels.FirstOrDefault(p => p.Id == parcelId);

            if (parcel == null)
                throw new NullReferenceException();

            parcel.SenderLockerId = senderLockerId;
            parcel.AddDepositEvent();
        }

        public IList<Parcel> ListParcels()
        {
            return _parcels;
        }

        public Parcel? GetParcel(int parcelId)
        {
            return _parcels.FirstOrDefault(p => p.Id == parcelId);
        }

        public void ForwardInTransit(Parcel parcel)
        {
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
        }
    }
}
