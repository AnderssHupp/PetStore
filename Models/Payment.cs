using Loja.Models.Enums;

namespace Loja.Models
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public required Order Order { get; set; }
        public decimal  Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string Provider { get; set; } = null!;
        public DateTime? PaidAt { get; set; }
    }
}
