using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransaction(Transaction data);
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction> GetTransactionById(int id);
        Task UpdateTransaction(Transaction data);
        Task DeleteTransaction(int id);
        Task<List<Transaction>> GetTransactionByUserId(int userId);
    }
}
