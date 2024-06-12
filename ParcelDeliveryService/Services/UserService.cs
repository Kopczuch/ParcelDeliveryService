using ParcelDeliveryService.Interfaces;
using ParcelDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelDeliveryService.Services
{
    public class UserService : IUserService
    {
        private User? _currentUser;
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool RegisterUser(User user)
        {
            var foundUser = _userRepository.GetAll().FirstOrDefault(u => u.Email == user.Email);
            if (foundUser == null)
            {
                _userRepository.Add(user);
                return true;
            }
            return false;
        }

        public bool LoginUser(string email, string password)
        {
            var foundUser = _userRepository.GetAll().FirstOrDefault(u => u.Email == email && u.Password == password);
            if (foundUser != null)
            {
                _currentUser = foundUser;
                return true;
            }
            return false;
        }

        public User? GetUser(int id)
        {
            return _userRepository.GetById(id);
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public IList<User> ListUsers()
        {
            return (List<User>)_userRepository.GetAll();
        }
    }
}
