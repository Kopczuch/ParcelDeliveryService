using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Interfaces
{
    public interface IChangeParcelStateCommand
    {
        void Execute(Parcel parcel);
    }
}
