namespace TarjetaCPruebaAPI.Models
{
    public class UserWithCreditCard
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CardNumber { get; set; } // Permitir nulo si el usuario no tiene tarjeta

        public List<CreditCard>? CreditCards { get; set; } // Esta propiedad debe estar presente si un usuario puede tener múltiples tarjetas
    }

}
