using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public abstract class ParcelState
    {
        public abstract bool IsTerminalState { get; }
        public abstract void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService);

        public virtual void Lose(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            var command = new LoseParcelCommand(parcelService, lockerService);
            command.Execute(parcel);

            Console.WriteLine("Parcel has been lost.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        public virtual void Destroy(Parcel parcel, IParcelService parcelService, ILockerService lockerService)
        {
            var command = new DestroyParcelCommand(parcelService, lockerService);
            command.Execute(parcel);

            Console.WriteLine("Parcel has been destroyed.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

    }
}
