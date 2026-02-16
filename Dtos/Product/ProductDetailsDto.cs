namespace Loja.Dtos.Product
{
    public record ProductDetailsDto
        (
            int Id,
            string Name,
            string Description,
            int CategoryId,
            string CategoryName,
            decimal Price,
            int Stock,
            bool IsActive
        );
}
