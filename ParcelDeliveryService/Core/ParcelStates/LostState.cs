using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class LostState : ParcelState
    {
        public override bool IsTerminalState => true;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            // note: there are no more actions possible in terminal state
        }

        public override void Destroy(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Lost parcel cannot be destroyed.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        public override void Lose(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Parcel was already lost.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
