using DAL.Models;
namespace FinanceAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        //List a user's transactions
        Task<List<Transaction>> GetMyTransactions(int userId);
        //Create a transaction
        Task CreateTransaction(Transaction data);
        //Update Transaction
        Task UpdateTransaction(Transaction data);

        //Delete Transaction
        Task DeleteTransaction(int id);

        //All transactions
        Task<List<Transaction>> GetAllTransactions();

        //Get transaction by id
        Task<Transaction?> GetTransactionById(int id);


    }
}
