namespace Loja.Dtos.User
{
    public record UserSummaryDto
        (
            int Id,
            string Name, 
            string Email,
            int RoleId,
            string RoleName,
            bool IsActive
        );
}
