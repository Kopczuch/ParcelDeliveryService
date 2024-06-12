using ParcelDeliveryService.Core.ParcelStates;
using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Commands
{
    public class DepositParcelCommand(IParcelService parcelService) : IChangeParcelStateCommand
    {
        public void Execute(Parcel parcel)
        {
            var transitEvent = new TransitEvent
            {
                TimeStamp = DateTime.Now,
                Location = $"Locker #{parcel.SenderLockerId}",
                Type = TransitEventType.Deposited
            };

            parcel.State = new DepositedState();
            parcel.TransitHistory.Add(transitEvent);
            
            parcelService.UpdateParcel(parcel);
        }
    }
}
