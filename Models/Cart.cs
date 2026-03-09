namespace Loja.Models
{
    public class Cart : BaseEntity
    {
        public int? UserId { get; set; }  // Nullable para carrinhos temporários
        public User? User { get; set; }

        public string? SessionId { get; set; }  // Para carrinhos temporários

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}
