using ParcelDeliveryService.Commands;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public abstract class ParcelState
    {
        public abstract bool IsTerminalState { get; }
        public abstract void HandleForwardInTransit(Parcel parcel, IParcelService parcelService, ILockerService lockerService);

        public virtual void Lose(Parcel parcel, IParcelService parcelService)
        {
            var command = new LoseParcelCommand(parcelService);
            command.Execute(parcel);

            Console.WriteLine("Parcel has been lost.\n");
        }

        public virtual void Destroy(Parcel parcel, IParcelService parcelService)
        {
            var command = new DestroyParcelCommand(parcelService);
            command.Execute(parcel);

            Console.WriteLine("Parcel has been destroyed.\n");
        }

    }
}
