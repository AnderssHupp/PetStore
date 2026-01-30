

namespace Loja.Dtos
{
    public record ProductDto(
        int Id,
        string Name, 
        string Description,
        string Category,
        decimal Price
     );
}