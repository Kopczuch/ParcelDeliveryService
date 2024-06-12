using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class DepositedState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            lockerService.ReceiveFromLocker(parcel.Id, parcel.SenderLockerId!.Value);
            parcel.AddReceivedFromSenderLockerEvent();
        }

        public override void Lose(Parcel parcel)
        {
            Console.WriteLine("Parcel cannot be lost since it has just been deposited.\n");
        }

    }
}
