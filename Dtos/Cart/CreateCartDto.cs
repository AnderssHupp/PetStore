
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Cart
{
    public record CreateCartDto
    (
        [Required] int UserId,
        [Required][MinLength(1)] List<CreateCartItemDto> Items

    );
    public record CreateCartItemDto(
        [Required] int ProductId,
        [Required] int Quantity
    );
}
