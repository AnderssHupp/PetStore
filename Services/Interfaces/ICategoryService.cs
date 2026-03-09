using Loja.Dtos.Category;

namespace Loja.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<CategoryDto?> GetByIdAsync(int id);

        Task<CategoryDto> CreateAsync(CreateCategoryDto createDto);

        Task<bool> UpdateAsync(int id, UpdateCategoryDto updatedDto);

        Task<bool> DeleteAsync(int id);
    }
}

