using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Payment
{
    public record UpdatePaymentStatusDto(
        [Required] PaymentStatus Status
        );
}

