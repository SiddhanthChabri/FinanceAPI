namespace FinanceAPI.DTOs
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        public string CategoryName { get; set; }
    }
}
