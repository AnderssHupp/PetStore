namespace Loja.Dtos.Cart
{
    public record CartDetailsDto(
        int Id,
        int? UserId,
        IReadOnlyCollection<CartItemDetailsDto> Items,
        decimal SubTotal
    );
    public record CartItemDetailsDto(
        int ProductId,
        string ProductName,
        decimal Price,
        int Quantity

    );
}