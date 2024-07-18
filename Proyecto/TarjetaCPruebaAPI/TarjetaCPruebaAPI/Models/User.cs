using System;
using System.Collections.Generic;

namespace TarjetaCPruebaAPI.Models
{
    public partial class User
    {
        public User()
        {
            CreditCards = new HashSet<CreditCard>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<CreditCard> CreditCards { get; set; }
    }
}
