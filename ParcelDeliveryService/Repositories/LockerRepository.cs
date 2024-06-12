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
    public class LockerRepository : ILockerRepository
    {
        private readonly List<Locker> _lockers;

        public LockerRepository()
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


        public void Add(Locker locker)
        {
            locker.Id = _lockers.Count;
            _lockers.Add(locker);  
        }

        public Locker? GetById(int id)
        {
            return _lockers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Locker> GetAll()
        {
            return _lockers;
        }

        public void Update(Locker locker)
        {
            int index = _lockers.FindIndex(l =>  l.Id == locker.Id);
            if (index != -1)
            {
                _lockers.Insert(index, locker);
            }
        }

        public void Remove(int id)
        {
            var locker = GetById(id);
            if (locker != null)
            {
                _lockers.Remove(locker);
            }
        }
    }
}
