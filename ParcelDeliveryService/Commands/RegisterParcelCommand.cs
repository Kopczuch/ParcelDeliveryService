using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class RegisterParcelCommand(IParcelService parcelService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Sender",
                Type = TransitEventType.Registered
            };

            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
        }
    }
}
