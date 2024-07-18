using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TarjetaCPruebaAPI.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int CardId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionType { get; set; }

        [JsonIgnore]
        public virtual CreditCard? Card { get; set; }
    }
}
