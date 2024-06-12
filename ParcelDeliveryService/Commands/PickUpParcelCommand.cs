using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class PickUpParcelCommand(IParcelService parcelService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Recipient",
                Type = TransitEventType.PickedUp
            };

            parcel.State = new PickedUpState();
            parcel.ActualPickUpTime = DateTime.Now;
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
        }
    }
}
