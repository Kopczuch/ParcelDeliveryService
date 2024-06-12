using ParcelDeliveryService.Interfaces;

namespace ParcelDeliveryService.Models.Parcels.ParcelStates
{
    public abstract class ParcelState
    {
        public abstract bool IsTerminalState { get; }
        public abstract void HandleForwardInTransit(Parcel parcel, ILockerService lockerService);

        public virtual void Lose(Parcel parcel)
        {
            parcel.AddLostEvent();
            Console.WriteLine("Parcel has been lost.\n");
        }

        public virtual void Destroy(Parcel parcel)
        {
            parcel.AddDestroyedEvent();
            Console.WriteLine("Parcel has been destroyed.\n");
        }

    }
}
