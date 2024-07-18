using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarjetaCPruebaAPI.Models; // Ajusta el espacio de nombres según tu aplicación
using TarjetaCPruebaAPI.Data; // Ajusta el espacio de nombres según tu aplicación

namespace TarjetaCPruebaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly CreditCardDBContext _context;

        public TransactionsController(CreditCardDBContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpPost("SaveTransaction")]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            if (transaction == null || transaction.CardId <= 0 || transaction.Amount <= 0)
            {
                return BadRequest("Invalid transaction data.");
            }

            // Encontrar la tarjeta de crédito asociada a la transacción
            var creditCard = await _context.CreditCards.FindAsync(transaction.CardId);
            if (creditCard == null)
            {
                return NotFound("Credit card not found.");
            }

            using var transactionScope = await _context.Database.BeginTransactionAsync();
            try
            {
                if (transaction.TransactionType == "Compra")
                {
                    // Validar que la compra no exceda el saldo disponible
                    if (transaction.Amount > creditCard.AvailableBalance)
                    {
                        return BadRequest("Transaction amount exceeds available balance.");
                    }

                    // Actualizar el balance de la tarjeta para una compra
                    creditCard.CurrentBalance += transaction.Amount;
                    creditCard.AvailableBalance -= transaction.Amount;
                }
                else if (transaction.TransactionType == "Abono" || transaction.TransactionType == "Pago")
                {
                    // Actualizar el balance de la tarjeta para un abono o pago
                    creditCard.CurrentBalance -= transaction.Amount;
                    creditCard.AvailableBalance += transaction.Amount;
                }
                else
                {
                    return BadRequest("Invalid transaction type.");
                }

                _context.Entry(creditCard).State = EntityState.Modified;

                // Guardar la transacción
                //_context.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertarPago @CardId={transaction.CardId},@TransactionDate={transaction.TransactionDate},@Description={transaction.Description},@Amount={transaction.Amount},@TransactionType={transaction.TransactionType}");
                //_context.Database.ExecuteSqlInterpolatedAsync($"EXEC proc @CardId={1},@TransactionDate={2024-07-17,@Amount={100.00},@TransactionType{Pago}");
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                await transactionScope.CommitAsync();

                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
            }
            catch (Exception ex)
            {
                await transactionScope.RollbackAsync();
                // Log the exception (ex) as needed
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }





        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }

        // GET: api/Transactions/card/5
        [HttpGet("card/{cardId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByCardId(int cardId)
        {
            return await _context.Transactions.Where(t => t.CardId == cardId).ToListAsync();
        }
    }
}

