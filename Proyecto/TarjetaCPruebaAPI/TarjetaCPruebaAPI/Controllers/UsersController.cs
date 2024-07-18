using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarjetaCPruebaAPI.Models;
using TarjetaCPruebaAPI.Data; 

namespace TarjetaCPruebaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CreditCardDBContext _context;

        public UsersController(CreditCardDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context
                .Users
                .Include(x => x.CreditCards)
                .FirstOrDefaultAsync(y => y.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        [HttpGet("userswithcreditcards")]
        public async Task<ActionResult<IEnumerable<UserWithCreditCard>>> GetUsersWithCreditCards()
        {
            var usersWithCreditCards = await _context.Users
                .Join(
                    _context.CreditCards,
                    user => user.UserId,
                    card => card.UserId,
                    (user, card) => new UserWithCreditCard
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CardNumber = card.CardNumber,
                    }
                )
                .ToListAsync();

            return usersWithCreditCards;
        }


        [HttpGet("userStatement")]
        public async Task<ActionResult<Statement>> GetUserStatement(int userId)
        {
            var user =
                _context
                .Users
                .Where(
                    x => 
                    x.UserId == userId)
                .Include(
                    y => 
                    y.CreditCards)
                .FirstOrDefault();

            var creditCardTransactions =
                _context
                .Transactions
                .Where(
                    x =>
                    x.CardId == user.CreditCards.FirstOrDefault().CardId)
                .ToList();

            var transactions = new List<TransactionStatement>();

            var userStatement = new Statement()
            {
                CustomerName = $"{user.FirstName} {user.LastName}",
                CreditCardNumber = user.CreditCards.FirstOrDefault().CardNumber,
                CardBalance = user.CreditCards.FirstOrDefault().CurrentBalance,
                CardLimit = user.CreditCards.FirstOrDefault().CreditLimit,
                BonusInterest = 0,
                AvailableBalance = user.CreditCards.FirstOrDefault().AvailableBalance
            };

            int counter = 1;
            foreach (var transaction in creditCardTransactions)
            {
                transactions.Add(new TransactionStatement()
                {
                    AuthorizationNumber = counter.ToString().PadLeft(10, '0'),
                    TransactionDate = transaction.TransactionDate,
                    TransactionAmount = transaction.Amount,
                    TransactionDescription = transaction.Description
                });

                counter++;
            }

            userStatement.TransactionStatements = transactions;

            return userStatement;
        }

    }
}
