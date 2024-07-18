using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarjetaCPruebaAPI.Models
{
    public partial class CreditCard
    {
        public CreditCard()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int CardId { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; } = null!;
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal? AvailableBalance { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
