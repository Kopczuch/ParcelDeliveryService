using System.Collections.Generic;
using ParcelDeliveryService.Core;

namespace ParcelDeliveryService.Models
{
    public class LockerComposite
    {
        public Locker Locker { get; }
        public List<Slot> AdditionalSlots { get; }

        public LockerComposite(Locker locker)
        {
            Locker = locker;
            AdditionalSlots = new List<Slot>();
        }

        public void AddSlot(Slot slot)
        {
            AdditionalSlots.Add(slot);
        }

        public void RemoveSlot(Slot slot)
        {
            AdditionalSlots.Remove(slot);
        }
    }
}
