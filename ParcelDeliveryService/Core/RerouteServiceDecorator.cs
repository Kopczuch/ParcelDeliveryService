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
            // Add a reroute event to the parcel's transit history
            parcel.TransitHistory.Add(new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Rerouted to Locker #{newLockerId}",
                Type = TransitEventType.InTransit
            });

            // Update the parcel's recipient locker ID
            parcel.RecipientLockerId = newLockerId;

            // Update the parcel in the parcel service
            _parcelService.UpdateParcel(parcel);
        }
    }
}
