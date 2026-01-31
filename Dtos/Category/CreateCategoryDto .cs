using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Category
{
    public record CreateCategoryDto
        (
            [Required][StringLength(50, MinimumLength = 3)] string Name
        );
}
