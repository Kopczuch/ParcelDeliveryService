using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(int id);
        IEnumerable<User> GetAll();
        void Update(User user);
        void Remove(int id);
    }
}
