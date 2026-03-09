using Loja.Models.Enums;

namespace Loja.Dtos.Order
{
    public record OrderDetailsDto(
        int Id,
        int UserId,
        int StatusId,
        string Status,
        decimal TotalAmount,
        DateTime CreatedAt,
        IReadOnlyCollection<OrderItemDetailsDto> Items
        );
    public record OrderItemDetailsDto(
        int ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity,
        decimal SubTotal
        );

}
