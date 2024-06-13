using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class DeliverParcelCommand(
        IParcelService parcelService,
        ILockerService lockerService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{parcel.RecipientLockerId}",
                Type = TransitEventType.ReadyForPickUp
            };

            parcel.ActualDeliveryTime = DateTime.Now;
            parcel.State = new ReadyForPickupState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
            lockerService.DepositParcel(parcel, parcel.RecipientLockerId);
        }
    }
}
