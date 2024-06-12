using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class TransitParcelCommand(IParcelService parcelService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In Transit",
                Type = TransitEventType.InTransit
            };

            parcel.State = new InTransitState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
        }
    }
}
