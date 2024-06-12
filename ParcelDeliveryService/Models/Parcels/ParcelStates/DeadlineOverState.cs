using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Models.Parcels.ParcelStates
{
    public class DeadlineOverState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            parcel.AddInExternalStorageEvent();
        }

        public override void Lose(Parcel parcel)
        {
            Console.WriteLine("Parcel cannot be lost.\n");
        }

        public override void Destroy(Parcel parcel)
        {
            Console.WriteLine("Parcel cannot be destroyed.\n");
        }
    }
}
