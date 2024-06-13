using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class StoreParcelExternallyCommand(
        IParcelService parcelService,
        ILockerService lockerService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = "In External Storage",
                Type = TransitEventType.InExternalStorage
            };

            parcel.State = new InExternalStorageState();
            parcel.TransitHistory.Add(transitEvent);

            parcelService.UpdateParcel(parcel);
            lockerService.ReceiveFromLocker(parcel.Id, parcel.RecipientLockerId);
        }
    }
}
