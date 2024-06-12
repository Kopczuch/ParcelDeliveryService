using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class DeadlineOverState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            var command = new StoreParcelExternallyCommand(parcelService, lockerService);
            command.Execute(parcel);
        }

        public override void Lose(Parcel parcel, IParcelService parcelService)
        {
            Console.WriteLine("Parcel cannot be lost.\n");
        }

        public override void Destroy(Parcel parcel, IParcelService parcelService)
        {
            Console.WriteLine("Parcel cannot be destroyed.\n");
        }
    }
}
