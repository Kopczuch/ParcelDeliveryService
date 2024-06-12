using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Models.Parcels.ParcelStates
{
    public class InStorageState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            parcel.AddInTransitEvent();
        }
    }
}
