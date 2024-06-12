using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class DestroyedState : ParcelState
    {
        public override bool IsTerminalState => true;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            // note: there are no more actions possible in terminal state
        }

        public override void Destroy(Parcel parcel)
        {
            Console.WriteLine("Parcel was already destroyed.\n");
            Console.ReadLine();
        }

        public override void Lose(Parcel parcel)
        {
            Console.WriteLine("Destroyed parcel cannot be lost.\n");
            Console.ReadLine();
        }
    }
}
