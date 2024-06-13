using ParcelDeliveryService.Models;
using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Core
{
    public class RerouteServiceDecorator : IRerouteService
    {
        private readonly IParcelService _parcelService;

        public RerouteServiceDecorator(IParcelService parcelService)
        {
            _parcelService = parcelService;
        }

        public void Reroute(Parcel parcel, int newLockerId)
        {
            parcel.TransitHistory.Add(new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Rerouted to Locker #{newLockerId}",
                Type = TransitEventType.InTransit
            });

            parcel.RecipientLockerId = newLockerId;

            _parcelService.UpdateParcel(parcel);
        }
    }
}
