
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Cart
{
    public record UpdateCartDto
    (
        [Required] int UserId,
        [Required] List<UpdateCartItemDto> Items
    );
    public record UpdateCartItemDto(
        [Required] int ProductId,
        [Required] int Quantity
    );
}
