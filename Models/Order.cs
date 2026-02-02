using Loja.Models.Enums;

namespace Loja.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public required User User { get; set; }
        public OrderStatus Status { get; set; } 
        public decimal Total { get; set; }

        public required Payment Payment { get; set; }

        public Shipment? Shipment { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
