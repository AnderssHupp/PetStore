using Loja.Models.Enums;

namespace Loja.Dtos.Payment
{
    public record PaymentDetailsDto(
        int Id,
        int OrderId,
        PaymentStatus Status,
        decimal Amount,
        string Provider,
        DateTime CreatedAt,
        DateTime? PaidAt
        );
}
