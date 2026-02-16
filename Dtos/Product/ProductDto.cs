namespace Loja.Dtos.Product
{
   public record ProductDto
    (
       int Id,
       string Name,
       string? Description,
       int CategoryId,
       decimal Price,
       bool IsActive
    );
}