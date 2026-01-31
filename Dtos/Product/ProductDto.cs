namespace Loja.Dtos.Product
{
    public record ProductDto
     (
        int Id,
        string Name, 
        string Description,
        string Category,
        decimal Price
     );
}