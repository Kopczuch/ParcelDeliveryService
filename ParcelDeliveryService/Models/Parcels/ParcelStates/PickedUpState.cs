using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Models.Parcels.ParcelStates
{
    public class PickedUpState : ParcelState
    {
        public override bool IsTerminalState => true;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            // note: there are no more actions possible in terminal state
        }

        public override void Destroy(Parcel parcel)
        {
            Console.WriteLine("Picked up parcel cannot be destroyed.\n");
        }

        public override void Lose(Parcel parcel)
        {
            Console.WriteLine("Picked up parcel cannot be lost.\n");
        }
    }
}
