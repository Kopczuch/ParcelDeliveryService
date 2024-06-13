using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class ReadyForPickupState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            var command = new ApproachPickupDeadlineCommand(parcelService);
            command.Execute(parcel);
        }

        public override void Lose(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Parcel cannot be lost.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        public override void Destroy(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            Console.WriteLine("Parcel cannot be destroyed.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
