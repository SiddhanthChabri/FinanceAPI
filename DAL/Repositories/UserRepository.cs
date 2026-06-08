using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task CreateUser(User data)
        {
            await _db.Users.AddAsync(data);
            await _db.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsers()
        {
            var res = await (from u in _db.Users select u).AsNoTracking().ToListAsync();
            return res;
        }
        public async Task<User> GetUserById(int id)
        {
            var res = await (from u in _db.Users select u).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }
        public async Task UpdateUser(User data)
        {
            _db.Users.Update(data);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            var res = await (from u in _db.Users select u).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            return res;
        }
    }
}
