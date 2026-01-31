using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Product
{
    public record UpdateProductDto
        (
            [Required][StringLength(50, MinimumLength = 3)] string Name,
            [StringLength(100)] string? Description,
            [Required][Range(1, 50)] int CategoryId,
            [Required][Range(1, 1000)] decimal Price,
            [Required] int Stock
        );
}
