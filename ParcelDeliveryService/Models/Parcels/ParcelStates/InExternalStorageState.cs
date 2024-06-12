using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Models.Parcels.ParcelStates
{
    public class InExternalStorageState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            base.Destroy(parcel);
        }
    }
}
