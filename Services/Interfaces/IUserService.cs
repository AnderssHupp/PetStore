using Loja.Dtos.User;

namespace Loja.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDetailsDto>> GetAllAsync();
        Task<UserDetailsDto?> GetByIdAsync(int id);
        Task<UserDetailsDto> CreateAsync(CreateUserDto createDto);
        Task<bool> UpdateAsync(int id, UpdateUserDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
