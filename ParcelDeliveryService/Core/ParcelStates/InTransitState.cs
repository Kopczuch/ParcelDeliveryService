using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class InTransitState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            lockerService.DepositParcel(parcel, parcel.RecipientLockerId);
            parcel.AddReadyForPickUpEvent();
        }
    }
}
