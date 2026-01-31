using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Loja.Dtos.Shipment
{
    public record ShipmentDetailsDto(
        int Id,
        int OrderId,
        int AddressId,
        string AddressName,
        ShipmentStatus Status,
        string? TrackingCode,
        DateTime CreatedAt,
        DateTime? DeliveredAt
        );
}
