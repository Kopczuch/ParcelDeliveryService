using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Interfaces
{
    public interface IRerouteService
    {
        void Reroute(Parcel parcel, int newLockerId);
    }
}
