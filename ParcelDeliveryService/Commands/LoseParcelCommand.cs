using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using ParcelDeliveryService.Services;

namespace ParcelDeliveryService.Commands
{
    public class LoseParcelCommand(IParcelService parcelService, ILockerService lockerService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "Unknown",
                Type = TransitEventType.Lost
            };

            parcel.State = new LostState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);

            OnLoseParcel(parcel);
        }

        private void OnLoseParcel(Parcel parcel)
        {
            lockerService.ReceiveFromLocker(parcel.Id, parcel.RecipientLockerId);
            lockerService.ReceiveFromLocker(parcel.Id, parcel.SenderLockerId!.Value);
        }
    }
}
