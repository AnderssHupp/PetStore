using System.ComponentModel.DataAnnotations;

namespace Loja.Models
{
    public class Stock : BaseEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        public void DecreaseQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }
            Quantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }
            Quantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
