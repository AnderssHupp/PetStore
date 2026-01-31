using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Order
{
    public record UpdateStatusOrderDto
    (
        [Required] OrderStatus Status
    );
   
}
