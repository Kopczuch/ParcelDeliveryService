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

        public void UpdateParcel(Parcel parcel)
        {
            // Update the parcel in the list (assuming in-memory storage)
            var existingParcel = _parcels.FirstOrDefault(p => p.Id == parcel.Id);
            if (existingParcel != null)
            {
                _parcels.Remove(existingParcel);
                _parcels.Add(parcel);
            }
        }
    }
}
