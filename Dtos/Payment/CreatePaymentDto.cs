using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Payment
{
    public record CreatePaymentDto(
        [Required] int OrderId,
        [Required] decimal Amount,
        [Required] string Provider
    );
}


