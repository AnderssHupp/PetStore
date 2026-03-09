using Loja.Dtos.Product;

namespace Loja.Services.Interfaces
{
    public interface IProductService
    {
        //busca varios produtos apenas para leitura
        Task<IEnumerable<ProductDetailsDto>> GetAllAsync();
        //buscar produto
        Task<ProductDetailsDto?> GetByIdAsync(int id);
        //criar produto
        Task<ProductDetailsDto> CreateAsync(CreateProductDto createDto);
        //bool or false, bool atualizou false n encontrou
        Task<bool> UpdateAsync(int id, UpdateProductDto updateDto);
        //Eliminar produto
        Task<bool> DeleteAsync(int id);

    }
}
