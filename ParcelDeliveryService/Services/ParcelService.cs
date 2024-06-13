using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

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

        public IList<Parcel> ListParcels()
        {
            return (IList<Parcel>)_parcelRepository.GetAll();
        }

        public Parcel? GetParcel(int parcelId)
        {
            return _parcelRepository.GetById(parcelId);
        }

        public void UpdateParcel(Parcel parcel)
        {
            _parcelRepository.Update(parcel);
        }
    }
}
