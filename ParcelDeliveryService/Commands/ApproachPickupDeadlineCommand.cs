using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class ApproachPickupDeadlineCommand(IParcelService parcelService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{parcel.RecipientLockerId}",
                Type = TransitEventType.DeadlineOver
            };

            parcel.State = new DeadlineOverState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
        }
    }
}
