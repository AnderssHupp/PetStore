using Loja.Models;
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Order
{
    public record CreateOrderDto
    (
        [Required] int UserId,
        [Required][MinLength(1)] List<OrderItem> Items

    );
}
