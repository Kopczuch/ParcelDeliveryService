﻿using ParcelDeliveryService.Core;
using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;

namespace ParcelDeliveryService.Repositories
{
    public class ParcelRepository : IParcelRepository
    {
        private readonly List<Parcel> _parcels;

        public ParcelRepository()
        {
            _parcels = new List<Parcel> {
                new Parcel(1, "James", "Edward", Size.Small, 1)
                {
                    TransitHistory = new List<TransitEvent>
                    {
                        new() {
                            TimeStamp = DateTime.Now.AddDays(-3),
                            Location = "At Sender",
                            Type = TransitEventType.Registered
                        }
                    }
                },
                new Parcel(2, "Samantha", "Andrew", Size.Large, 3)
                {
                    TransitHistory = new List<TransitEvent>
                    {
                        new() {
                            TimeStamp = DateTime.Now.AddDays(-1),
                            Location = "At Sender",
                            Type = TransitEventType.Registered
                        }
                    }
                },
                new Parcel(3, "Andrew", "James", Size.Medium, 2)
                {
                    TransitHistory = new List<TransitEvent>
                    {
                        new() {
                            TimeStamp = DateTime.Now.AddDays(-2),
                            Location = "At Sender",
                            Type = TransitEventType.Registered
                        }
                    }
                }
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
