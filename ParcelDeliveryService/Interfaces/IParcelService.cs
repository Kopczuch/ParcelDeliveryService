using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Interfaces
{
    public interface IParcelService
    {
        void RegisterParcel(Parcel parcel);
        void DepositParcel(int parcelId, int lockerId);
        IList<Parcel> ListParcels();
        Parcel? GetParcel(int parcelId);

    }
}
