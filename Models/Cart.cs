namespace Loja.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public required User User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}
