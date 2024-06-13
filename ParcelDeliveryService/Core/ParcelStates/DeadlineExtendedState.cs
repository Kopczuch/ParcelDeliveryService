using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class DeadlineExtendedState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            var command = new StoreParcelExternallyCommand(parcelService, lockerService);
            command.Execute(parcel);
        }

    }
}
