using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class DepositedState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            var command = new ReceiveFromSenderLockerCommand(parcelService, lockerService);
            command.Execute(parcel);
        }

        public override void Lose(Parcel parcel, IParcelService parcelService)
        {
            Console.WriteLine("Parcel cannot be lost since it has just been deposited.\n");
        }

    }
}
