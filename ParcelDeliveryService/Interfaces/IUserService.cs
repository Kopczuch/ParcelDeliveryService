using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Interfaces
{
    public interface IUserService
    {
        bool RegisterUser(User user);
        bool LoginUser(string email, string password);
        User? GetUser(int id);
        User? GetCurrentUser();
        IList<User> ListUsers();
    }
}
