using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Interfaces
{
    public interface ILockerRepository
    {
        void Add(Locker locker);
        Locker? GetById(int id);
        IEnumerable<Locker> GetAll();
        void Update(Locker locker);
        void Remove(int id);
    }
}
