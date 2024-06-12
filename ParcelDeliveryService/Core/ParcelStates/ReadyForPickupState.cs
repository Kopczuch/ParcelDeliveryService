﻿using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Core.ParcelStates
{
    public class ReadyForPickupState : ParcelState
    {
        public override bool IsTerminalState => false;

        public override void HandleForwardInTransit(Parcel parcel, ILockerService lockerService)
        {
            parcel.AddDeadlineOverEvent();
        }

        public override void Lose(Parcel parcel)
        {
            Console.WriteLine("Parcel cannot be lost.\n");
        }

        public override void Destroy(Parcel parcel)
        {
            Console.WriteLine("Parcel cannot be destroyed.\n");
        }
    }
}
