using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.User
{
    public record UpdateUserDto
    (
        [Required][StringLength(50, MinimumLength = 3)] string Name,
        [Required][EmailAddress] string Email,
        [Required][MinLength(8)] string Password,
        [Required] UserRole RoleId, 
        bool IsActive
    );
    
}
