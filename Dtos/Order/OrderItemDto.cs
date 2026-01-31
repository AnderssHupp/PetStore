using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Order
{
    public record OrderItemDto
    (
        [Required] int ProductId,
        [Required][Range(1, 1000)] int Quantity
    );
}
