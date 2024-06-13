using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Services
{
    public class LockerService : ILockerService
    {
        private ILockerRepository _lockerRepository;

        public LockerService(ILockerRepository lockerRepository)
        {
            _lockerRepository = lockerRepository;
        }

        public IList<Locker> GetVacantLockers()
        {
            return _lockerRepository.GetAll().Where(l => l.Slots.Any(s => s.Vacancy == VacancyState.Vacant)).ToList();
        }

        public IList<Locker> GetLockers()
        {
            return (IList<Locker>)_lockerRepository.GetAll();
        }

        public int DepositParcel(Parcel parcel, int lockerId)
        {
            var locker = _lockerRepository.GetById(lockerId);

            if (locker == null)
                throw new NullReferenceException();

            var slotId = locker.Deposit(parcel);
            _lockerRepository.Update(locker);

            return slotId;
        }

        public void ReserveSlot(Parcel parcel, int lockerId)
        {
            var locker = _lockerRepository.GetById(lockerId);

            if (locker == null)
                throw new NullReferenceException();

            locker.ReserveSlot(parcel);
            _lockerRepository.Update(locker);
        }

        public void ReleaseSlot(Parcel parcel, int lockerId)
        {
            var locker = _lockerRepository.GetById(lockerId);

            if (locker == null)
                throw new NullReferenceException();

            locker.ReleaseSlot(parcel);
            _lockerRepository.Update(locker);
        }

        public bool ReceiveFromLocker(int parcelId, int lockerId)
        {
            var locker = _lockerRepository.GetById(lockerId);

            if (locker == null)
                throw new NullReferenceException();

            bool output = locker.ReceiveFromLocker(parcelId);
            if(output)
            {
                _lockerRepository.Update(locker);
            }
            return output;
        }

        public bool ChangeAddress(int lockerId, Address address)
        {
            var locker = _lockerRepository.GetById(lockerId);

            if (locker == null)
                throw new NullReferenceException();

            if(locker.IsVacant())
            {
                locker.Address = address;
                _lockerRepository.Update(locker);
                return true;
            }

            return false;
        }
    }
}
