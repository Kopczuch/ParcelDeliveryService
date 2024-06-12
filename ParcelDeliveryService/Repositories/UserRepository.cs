using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository()
        {
            _users = new List<User> {
                new(1, "John", "Doe", "123456789", "john.doe@example.com", "password", 
                    new Address("Poland", "Warsaw", "00-001", "Main St", "1")),
                new(2, "Jane", "Smith", "987654321", "jane.smith@example.com", "password", 
                    new Address("Poland", "Krakow", "30-002", "Second St", "2")),
                new(3, "Bob", "Brown", "555555555", "bob.brown@example.com", "password", 
                    new Address("Poland", "Gdansk", "80-003", "Third St", "3")),
                new(4, "Alice", "Johnson", "111111111", "alice.johnson@example.com", "password", 
                    new Address("Poland", "Poznan", "60-004", "Fourth St", "4"))
            };
        }

        public void Add(User user)
        {
            user.Id = _users.Count;
            _users.Add(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void Remove(int id)
        {
            var user = GetById(id); 
            if (user != null) 
            {
                _users.Remove(user);
            } 
        }

        public void Update(User user)
        {
            int index = _users.FindIndex(u => u.Id == user.Id);
            if(index == -1)
            {
                _users.Insert(index, user);
            }
        }
    }
}
