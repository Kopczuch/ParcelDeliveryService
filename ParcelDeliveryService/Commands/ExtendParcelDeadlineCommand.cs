using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class ExtendParcelDeadlineCommand(IParcelService parcelService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In Storage",
                Type = TransitEventType.DeadlineExtended
            };

            parcel.State = new DeadlineExtendedState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
        }
    }
}
