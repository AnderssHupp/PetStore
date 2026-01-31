using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Category
{
    public record UpdateCategoryDto
        (
            [Required][StringLength(50, MinimumLength = 3)] string Name
        );
}
