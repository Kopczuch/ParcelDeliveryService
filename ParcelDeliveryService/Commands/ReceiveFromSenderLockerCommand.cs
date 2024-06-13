using ParcelDeliveryService.Core;
using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class ReceiveFromSenderLockerCommand(
        IParcelService parcelService,
        ILockerService lockerService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "At Courier",
                Type = TransitEventType.ReceivedFromSenderLocker
            };

            parcel.State = new ReceivedFromSenderLockerState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
            lockerService.ReceiveFromLocker(parcel.Id, parcel.SenderLockerId!.Value);
        }
    }
}
