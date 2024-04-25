using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Interfaces
{
    public interface IParcelService
    {
        Parcel RegisterParcel(Parcel parcel);
        void DepositParcel(int parcelId, int lockerId);
        IList<Parcel> ListParcels();
        Parcel? GetParcel(int parcelId);
        void ForwardInTransit(Parcel parcel);

    }
}
