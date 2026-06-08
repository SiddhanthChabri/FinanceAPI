using DAL.Models;

namespace FinanceAPI.DTOs
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public int CategoryId { get; set; }

    }
}
