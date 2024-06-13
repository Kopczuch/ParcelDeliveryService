using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class RegisteredState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Parcel has not been deposited yet.\n");
            Console.ReadLine();
        }

        public override void Lose(Parcel parcel, IParcelService parcelService)
        {
            Console.WriteLine("Parcel cannot be lost since it has not been deposited yet.\n");
            Console.ReadLine();

        }

        public override void Destroy(Parcel parcel, IParcelService parcelService)
        {
            Console.WriteLine("Parcel cannot be destroyed since it has not been deposited yet.\n");
            Console.ReadLine();
        }
    }
}
