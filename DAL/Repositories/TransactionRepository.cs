using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;
        public TransactionRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddTransaction(Transaction data)
        {
            await _db.Transactions.AddAsync(data);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _db.Transactions
                .Include(t => t.User)
                .Include(t => t.Category)
                .AsNoTracking()
                .ToListAsync();
        }
        //GetTransactionbyId
        public async Task<Transaction?> GetTransactionById(int id)
        {
            return await _db.Transactions
                .Include(t => t.User)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        //Update
        public async Task UpdateTransaction(Transaction data)
        {
            _db.Transactions.Update(data);
            await _db.SaveChangesAsync();
        }
        //Delete
        public async Task DeleteTransaction(int id)
        {
            var res = await (from t in _db.Transactions select t).FirstOrDefaultAsync(x => x.Id == id);
            if (res != null)
            {
                _db.Transactions.Remove(res);
                await _db.SaveChangesAsync();
            }
        }
        //Getbyuser
        public async Task<List<Transaction>> GetTransactionByUserId(int userId)
        {
            return await _db.Transactions
                .Include(t => t.User)
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }
}
