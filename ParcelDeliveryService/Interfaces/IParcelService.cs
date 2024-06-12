using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Interfaces
{
    public interface IParcelService
    {
        Parcel RegisterParcel(Parcel parcel);
        IList<Parcel> ListParcels();
        Parcel? GetParcel(int parcelId);
        void UpdateParcel(Parcel parcel);

    }
}
