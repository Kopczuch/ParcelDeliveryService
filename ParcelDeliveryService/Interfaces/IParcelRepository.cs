using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Interfaces
{
    public interface IParcelRepository
    {
        void Add(Parcel parcel);
        Parcel GetById(int id);
        IEnumerable<Parcel> GetAll();
        void Update(Parcel parcel);
        void Remove(int id);
    }
}
