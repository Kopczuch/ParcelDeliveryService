using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class DestroyParcelCommand(IParcelService parcelService, ILockerService lockerService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "Trash",
                Type = TransitEventType.Destroyed
            };

            parcel.State = new DestroyedState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);

            OnDestroyParcel(parcel);
        }

        private void OnDestroyParcel(Parcel parcel)
        {
            lockerService.ReceiveFromLocker(parcel.Id, parcel.RecipientLockerId);
            lockerService.ReceiveFromLocker(parcel.Id, parcel.SenderLockerId!.Value);
        }
    }
}
