using ParcelDeliveryService.Core;

namespace ParcelDeliveryService.Models
{
    public class Locker
    {
        public Locker()
        {
            Slots = new List<Slot>();
            LockerHistory = new List<LockerHistoryItem>();
        }

        public int Id { get; set; }
        public IList<Slot> Slots { get; set; }
        public IList<LockerHistoryItem> LockerHistory { get; set; }

        public int Deposit(Parcel parcel)
        {
            if (parcel.Size == Size.Small)
            {
                var slot = Slots.FirstOrDefault(s => s is { Vacancy: VacancyState.Vacant, Size: Size.Small });

                if (slot != null)
                {
                    slot.ParcelId = parcel.Id;
                    LockerHistory.Add(new LockerHistoryItem{ ParcelId = parcel.Id, SlotId = slot.Id, Deposited = DateTime.Now });
                    return slot.Id;
                }
            }

            if (parcel.Size != Size.Large)
            {
                var slot = Slots.FirstOrDefault(s => s is { Vacancy: VacancyState.Vacant, Size: Size.Medium });

                if (slot != null)
                {
                    slot.ParcelId = parcel.Id;
                    LockerHistory.Add(new LockerHistoryItem { ParcelId = parcel.Id, SlotId = slot.Id, Deposited = DateTime.Now });
                    return slot.Id;
                }
            }

            var vacantSlot = Slots.FirstOrDefault(s => s is { Vacancy: VacancyState.Vacant, Size: Size.Large });

            if (vacantSlot == null)
                throw new Exception("No slots available");

            vacantSlot.ParcelId = parcel.Id;
            LockerHistory.Add(new LockerHistoryItem { ParcelId = parcel.Id, SlotId = vacantSlot.Id, Deposited = DateTime.Now });

            return vacantSlot.Id;
        }

        public void ReserveSlot(Parcel parcel)
        {
            if (parcel.Size == Size.Small)
            {
                var slot = Slots.FirstOrDefault(s => s is { Vacancy: VacancyState.Vacant, Size: Size.Small });

                if (slot != null)
                {
                    slot.Vacancy = VacancyState.Reserved;
                    slot.ParcelId = parcel.Id;
                    
                    return;
                }
            }

            if (parcel.Size != Size.Large)
            {
                var slot = Slots.FirstOrDefault(s => s is { Vacancy: VacancyState.Vacant, Size: Size.Medium });

                if (slot != null)
                {
                    slot.Vacancy = VacancyState.Reserved;
                    slot.ParcelId = parcel.Id;
                    
                    return;
                }
            }

            var vacantSlot = Slots.FirstOrDefault(s => s is { Vacancy: VacancyState.Vacant, Size: Size.Large });

            if (vacantSlot == null)
                throw new Exception("No slots available");

            vacantSlot.Vacancy = VacancyState.Reserved;
            vacantSlot.ParcelId = parcel.Id;
        }

        public bool ReceiveFromLocker(int parcelId)
        {
            var slot = Slots.FirstOrDefault(s => s.ParcelId == parcelId);

            if (slot == null)
                return false;

            slot.Vacancy = VacancyState.Vacant;
            slot.ParcelId = null;

            return true;
        }

        private void UpdateHistoryItem(int parcelId, int slotId)
        {
            var historyItem = LockerHistory.FirstOrDefault(i => i.ParcelId == parcelId && i.SlotId == slotId);

            if (historyItem == null)
                throw new NullReferenceException();

            historyItem.Received = DateTime.Now;
        }

        public string GetOccupancy()
        {
            var occupiedSlots = Slots.Where(s => s.Vacancy != VacancyState.Vacant).ToList();

            return $"{occupiedSlots.Count}/{Slots.Count}";
        }

        public void Display()
        {
            Console.WriteLine($"Locker #{ Id }");
            Console.WriteLine($"Occupancy: {GetOccupancy()}");
        }

        internal void ReleaseSlot(Parcel parcel)
        {
            var slot = Slots.FirstOrDefault(s => s.ParcelId == parcel.Id);
            if (slot != null)
            {
                slot.Vacancy = VacancyState.Vacant;
                slot.ParcelId = null;
                return;
            }

        }
    }
}
