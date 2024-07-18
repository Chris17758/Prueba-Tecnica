namespace TarjetaCPruebaAPI.Models
{
    // Used as base reference for statement display
    public class Statement
    {
        public string? CustomerName { get; set; }
        public string? CreditCardNumber { get; set; }
        public decimal? CardBalance { get; set; }
        public decimal? CardLimit { get; set; }
        public decimal? BonusInterest { get; set; }
        public decimal? AvailableBalance { get; set; }
        public IEnumerable<TransactionStatement>? TransactionStatements { get; set; }
    }

    public class TransactionStatement
    {
        public string? AuthorizationNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal? TransactionAmount { get; set; }
    }
}