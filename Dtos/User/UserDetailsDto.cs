using Loja.Models.Enums;

namespace Loja.Dtos.User
{
    public record UserDetailsDto
        (
            int Id,
            string Name,
            string Email,
            int RoleId,
            string RoleName,
            bool IsActive
        );
}
