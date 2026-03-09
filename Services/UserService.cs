using Loja.Data;
using Loja.Models;
using Loja.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Loja.Services.Interfaces;

namespace Loja.Services
{
    public class UserService : IUserService
    {
        private readonly PetStoreContext _context;

        public UserService(PetStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDetailsDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserDetailsDto
                (
                    u.Id,
                    u.Name,
                    u.Email,
                    (int)u.Role,
                    u.Role.ToString(),
                    u.IsActive
                ))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserDetailsDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDetailsDto
                (
                    u.Id,
                    u.Name,
                    u.Email,
                    (int)u.Role,
                    u.Role.ToString(),
                    u.IsActive
                ))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<UserDetailsDto> CreateAsync(CreateUserDto newUser)
        {
            User user = new()
            {
                Name = newUser.Name,
                Email = newUser.Email,
                PasswordHash = newUser.Password,
                Role = newUser.RoleId,
                IsActive = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDetailsDto(
                user.Id,
                user.Name,
                user.Email,
                (int)user.Role,
                user.Role.ToString(),
                user.IsActive
            );
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto updateUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            user.Name = updateUser.Name;
            user.Email = updateUser.Email;
            user.Role = updateUser.RoleId;
            user.IsActive = updateUser.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            user.IsActive = false; // Soft delete
            user.DeletedAt = DateTime.UtcNow;
            //_context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
