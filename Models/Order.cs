using Loja.Models.Enums;

namespace Loja.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }

        public Payment? Payment { get; set; }

        public Shipment? Shipment { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }
            Items.Add(new OrderItem { Product = product, Quantity = quantity, PriceAtPurchase = product.Price });
            RecalculateTotal();
        }

        public void RecalculateTotal()
        {
            Total = Items.Sum(i => i.Quantity * i.PriceAtPurchase);
        }
    }
}
