using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Shipment
{
    public record CreateShipmentDto(
        [Required] int OrderId,
        [Required] int AddressId
        );
}
