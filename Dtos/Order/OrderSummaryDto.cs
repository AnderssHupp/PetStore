using Loja.Models.Enums;

namespace Loja.Dtos.Order
{
    public record OrderSummaryDto(
          int Id,
          OrderStatus Status,
          decimal TotalAmount,
          DateTime CreatedAt
      );
}
