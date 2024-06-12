using ParcelDeliveryService.Core;
using ParcelDeliveryService.Models;
using System;

namespace ParcelDeliveryService.ChainOfResponsibility
{
    
    public interface IDeadlineHandler
    {
        void HandleDeadline(Parcel parcel);
    }

    
    public class PickUpDeadlineHandler : IDeadlineHandler
    {
        private IDeadlineHandler? _nextHandler;

        public void SetNextHandler(IDeadlineHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public void HandleDeadline(Parcel parcel)
        {
            // Check if the parcel has been delivered
            if (parcel.CurrentState == TransitEventType.PickedUp)
            {
                // Check if the parcel was picked up within 48 hours
                if (DateTime.Now - parcel.ActualDeliveryTime <= TimeSpan.FromHours(48))
                {
                    Console.WriteLine("Parcel picked up within 48 hours. Deadline met.");
                }
                else
                {
                    Console.WriteLine("Parcel not picked up within 48 hours. Moving to the next deadline.");
                    _nextHandler.HandleDeadline(parcel);
                }
            }
            else
            {
                Console.WriteLine("Parcel has not been delivered yet. Cannot check pick-up deadline.");
            }
        }
    }

    public interface IDdeadlineHandler
    {
    }

    public class ExternalStorageDeadlineHandler : IDeadlineHandler
    {
        private IDeadlineHandler _nextHandler;

        public void SetNextHandler(IDeadlineHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public void HandleDeadline(Parcel parcel)
        {
            // Check if the parcel is in external storage
            if (parcel.CurrentState == TransitEventType.InExternalStorage)
            {
                // Check if the parcel has been in external storage for more than 5 days
                if (DateTime.Now - parcel.TransitHistory.Last(t => t.Type == TransitEventType.InExternalStorage).TimeStamp >= TimeSpan.FromDays(5))
                {
                    Console.WriteLine("Parcel in external storage for more than 5 days. Additional fee may apply for extension.");
                }
                else
                {
                    Console.WriteLine("Parcel in external storage for less than 5 days. Deadline not reached.");
                }
            }
            else
            {
                Console.WriteLine("Parcel is not in external storage. Cannot check external storage deadline.");
            }
        }
    }

    // Step 3: Configure the chain
    public class DeadlineHandlerChain
    {
        private readonly IDeadlineHandler _rootHandler;

        public DeadlineHandlerChain()
        {
            // Create and configure the chain of responsibility
            var pickUpHandler = new PickUpDeadlineHandler();
            var externalStorageHandler = new ExternalStorageDeadlineHandler();

            pickUpHandler.SetNextHandler(externalStorageHandler);

            _rootHandler = pickUpHandler;
        }

        public void HandleDeadline(Parcel parcel)
        {
            _rootHandler.HandleDeadline(parcel);
        }
    }
}
