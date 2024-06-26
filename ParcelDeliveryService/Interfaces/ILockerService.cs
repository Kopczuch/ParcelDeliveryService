﻿using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Interfaces
{
    public interface ILockerService
    {
        IList<Locker> GetVacantLockers();
        IList<Locker> GetLockers();
        int DepositParcel(Parcel parcel, int lockerId);
        void ReserveSlot(Parcel parcel, int lockerId);
        bool ReceiveFromLocker(int parcelId, int lockerId);
        void ReleaseSlot(Parcel parcel, int recipientLockerId);
        bool ChangeAddress(int parcelId, Address address);
    }
}
