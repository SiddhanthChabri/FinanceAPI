using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUser(User data);
        Task UpdateUser(User data);
        Task DeleteUser(int id);
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByEmail(string email);
    }
}
