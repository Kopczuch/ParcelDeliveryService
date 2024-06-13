using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class DestroyedState : ParcelState
    {
        public override bool IsTerminalState => true;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            // note: there are no more actions possible in terminal state
        }

        public override void Destroy(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Parcel was already destroyed.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        public override void Lose(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Destroyed parcel cannot be lost.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
