using FinanceAPI.Services.Interfaces;
using DAL.Models;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        //User trnasactions
        public async Task<List<Transaction>> GetMyTransactions(int userId)
        {
            return await _transactionRepository.GetTransactionByUserId(userId);
        }
        //Create Transactions
        public async Task CreateTransaction(Transaction data)
        {
            await _transactionRepository.AddTransaction(data);
        }
        //Update Transactions
        public async Task UpdateTransaction(Transaction data)
        {
            await _transactionRepository.UpdateTransaction(data);
        }
        //Delete Transactions
        public async Task DeleteTransaction(int id)
        {
            await _transactionRepository.DeleteTransaction(id);
        }
        //All Transactions
        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _transactionRepository.GetAllTransactions();
        }

        //Transaction by id
        public async Task<Transaction?> GetTransactionById(int id)
        {
            return await _transactionRepository.GetTransactionById(id);
        }
    }
}
