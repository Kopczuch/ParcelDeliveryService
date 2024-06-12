using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class LostState : ParcelState
    {
        public override bool IsTerminalState => true;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            // note: there are no more actions possible in terminal state
        }

        public override void Destroy(Parcel parcel)
        {
            Console.WriteLine("Lost parcel cannot be destroyed.\n");
            Console.ReadLine();
        }

        public override void Lose(Parcel parcel)
        {
            Console.WriteLine("Parcel was already lost.\n");
            Console.ReadLine();
        }
    }
}
