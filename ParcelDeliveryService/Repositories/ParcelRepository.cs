using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Repositories
{
    public class ParcelRepository : IParcelRepository
    {
        private readonly List<Parcel> _parcels;

        public ParcelRepository()
        {
            _parcels = new List<Parcel> {
                new Parcel(0, "James", "Edward", Size.Small, 1),
                new Parcel(1, "Samantha", "Andrew", Size.Large, 3),
                new Parcel(2, "Andrew", "James", Size.Medium, 2)
            };
        }

        public void Add(Parcel parcel)
        {
            parcel.Id = _parcels.Count;
            _parcels.Add(parcel);
        }

        public Parcel? GetById(int id)
        {
            return _parcels.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Parcel> GetAll()
        {
            return _parcels;
        }

        public void Update(Parcel parcel)
        {
            int index = _parcels.FindIndex(p => p.Id == parcel.Id);
            if (index != -1)
            {
                _parcels.Insert(index, parcel);
            }
        }

        public void Remove(int id)
        {
            var parcel = GetById(id);
            if (parcel != null)
            {
                _parcels.Remove(parcel);
            }
        }
    }

}
