namespace Loja.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public required User User { get; set; }

    }
}
