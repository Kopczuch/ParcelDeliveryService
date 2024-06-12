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
            _parcelRepository.Update(parcel);
        }
    }
}
