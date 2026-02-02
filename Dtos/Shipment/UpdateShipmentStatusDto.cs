using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Shipment
{
    public record UpdateShipmentStatusDto(
        [Required] ShipmentStatus Status
        );

}