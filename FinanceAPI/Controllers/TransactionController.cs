using DAL.Interfaces;
using DAL.Models;
using FinanceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        //moving from repository layer to service layer for better separation of concerns and to handle business logic in the service layer
        private readonly ITransactionService _transactionService;

        public TransactionController(
           ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllTransaction()
        {
            var transactions =
                await _transactionService.GetAllTransactions();

            return Ok(transactions);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionById(id);
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            if (transaction == null)
                return NotFound("Transaction not found");

            return Ok(transaction);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            FinanceAPI.DTOs.TransactionDto data)
        {
            throw new Exception("Testing Exception Middleware");
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var transaction = new Transaction
            {
                Amount = data.Amount,
                Description = data.Description,
                TransactionDate = data.TransactionDate,
                TransactionType = data.TransactionType,
                CategoryId = data.CategoryId,
                UserId = userId
            };

            await _transactionService.CreateTransaction(transaction);

            return Ok(transaction);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(
            int id,
            FinanceAPI.DTOs.TransactionDto data)
        {
            var transaction =
                await _transactionService.GetTransactionById(id);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            if (transaction == null)
                return NotFound("Transaction not found");

            transaction.Amount = data.Amount;
            transaction.Description = data.Description;
            transaction.TransactionDate = data.TransactionDate;
            transaction.TransactionType = data.TransactionType;
            transaction.CategoryId = data.CategoryId;

            await _transactionService.UpdateTransaction(transaction);

            return Ok(transaction);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction =
                await _transactionService.GetTransactionById(id);

            if (transaction == null)
                return NotFound("Transaction not found");


            await _transactionService.DeleteTransaction(id);

            return Ok("Transaction Deleted Successfully");
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyTransaction()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var transactions =
                await _transactionService.GetMyTransactions(userId);
            if(transactions.Any(t => t.UserId != userId))
            {
                return Unauthorized("You are not authorized to view these transactions");
            }

            return Ok(transactions);
        }
    }
}