using Loja.Data;
using Loja.Dtos.Category;
using Loja.Models;
using Loja.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Loja.Services
{
    public class CategoryService : ICategoryService
    {
        //conectar com o banco de dados
        private readonly PetStoreContext _context;
        public CategoryService(PetStoreContext context)
        {
            _context = context;
        }
        //get all categories
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _context.Categories.Select(c => new CategoryDto
            (
                c.Id,
                c.Name
            ))
            .AsNoTracking()
            .ToListAsync();
        }

        //get category by id
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto(
                c.Id,
                c.Name
                ))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto newCategory)
        {
            Category category = new()
            {
                Name = newCategory.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            (
                category.Id,
                category.Name
            );
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto updateDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            category.Name = updateDto.Name;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}