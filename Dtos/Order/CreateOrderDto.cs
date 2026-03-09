
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Order
{
    public record CreateOrderDto
    (
        [Required] int UserId,
        [Required][MinLength(1)] List<CreateOrderItemDto> Items

    );
    public record CreateOrderItemDto(
        [Required] int ProductId,
        [Required][Range(1, 1000)] int Quantity
    );
}
