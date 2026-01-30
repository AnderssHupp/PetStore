using Loja.Models.Enums;

namespace Loja.Models
{
    public class Shipment: BaseEntity
    {
        public int OrderId { get; set; }
        public required Order Order { get; set; }
        public int AdressId { get; set; }
        public required Address Address { get; set; }

        public string? TrackingCode { get; set; }

        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

        public DateTime DeliveredAt { get; set; }

    }   
}
