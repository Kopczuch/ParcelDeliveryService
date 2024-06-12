using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using ParcelDeliveryService.Models.Parcels;

namespace ParcelDeliveryService.Services
{
    public class LockerService : ILockerService
    {
        private readonly IList<Locker> _lockers;

        public LockerService()
        {
            _lockers = new List<Locker>
            {
                new()
                {
                    Id = 1,
                    Slots = new List<Slot>
                    {
                        new() { Id = 1, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 2, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 3, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 4, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 5, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 6, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 7, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 8, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 9, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 10, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 11, Size = Size.Large, Vacancy = VacancyState.Vacant },
                        new() { Id = 12, Size = Size.Large, Vacancy = VacancyState.Vacant }
                    }
                },

                new()
                {
                    Id = 2,
                    Slots = new List<Slot>
                    {
                        new() { Id = 1, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 2, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 3, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 4, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 5, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 6, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 7, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 8, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 9, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 10, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 11, Size = Size.Large, Vacancy = VacancyState.Vacant },
                        new() { Id = 12, Size = Size.Large, Vacancy = VacancyState.Vacant }
                    }
                },

                new()
                {
                    Id = 3,
                    Slots = new List<Slot>
                    {
                        new() { Id = 1, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 2, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 3, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 4, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 5, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 6, Size = Size.Small, Vacancy = VacancyState.Vacant },
                        new() { Id = 7, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 8, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 9, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 10, Size = Size.Medium, Vacancy = VacancyState.Vacant },
                        new() { Id = 11, Size = Size.Large, Vacancy = VacancyState.Vacant },
                        new() { Id = 12, Size = Size.Large, Vacancy = VacancyState.Vacant }
                    }
                }
            };
        }

        public IList<Locker> GetVacantLockers()
        {
            return _lockers.Where(l => l.Slots.Any(s => s.Vacancy == VacancyState.Vacant)).ToList();
        }

        public IList<Locker> GetLockers()
        {
            return _lockers;
        }

        public int DepositParcel(Parcel parcel, int lockerId)
        {
            var locker = _lockers.FirstOrDefault(l => l.Id == lockerId);

            if (locker == null)
                throw new NullReferenceException();

            var slotId = locker.Deposit(parcel);

            return slotId;
        }

        public void ReserveSlot(Parcel parcel, int lockerId)
        {
            var locker = _lockers.FirstOrDefault(l => l.Id == lockerId);

            if (locker == null)
                throw new NullReferenceException();

            locker.ReserveSlot(parcel);
        }

        public bool ReceiveFromLocker(int parcelId, int lockerId)
        {
            var locker = _lockers.FirstOrDefault(l => l.Id == lockerId);

            if (locker == null)
                throw new NullReferenceException();

            return locker.ReceiveFromLocker(parcelId);
        }
    }
}
