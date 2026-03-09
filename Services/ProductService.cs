using Loja.Data;
using Loja.Dtos.Product;
using Loja.Models;
using Loja.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Loja.Services
{
    public class ProductService(PetStoreContext context, IStockService stockService) : IProductService
    {
        private readonly PetStoreContext _context = context;
        private readonly IStockService _stockService = stockService;

        public async Task<IEnumerable<ProductDetailsDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .Select(p => new ProductDetailsDto(
                    p.Id,
                    p.Name,
                    p.Description ?? "",
                    (int)p.CategoryId,
                    p.Category != null ? p.Category.Name.ToString() : "",
                    p.Price,
                    p.Stock != null ? p.Stock.Quantity : 0,
                    p.IsActive
                ))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsDto(
                    p.Id,
                    p.Name,
                    p.Description ?? "",
                    (int)p.CategoryId,
                    p.Category != null ? p.Category.Name.ToString() : "",
                    p.Price,
                    p.Stock != null ? p.Stock.Quantity : 0,
                    p.IsActive
                ))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<ProductDetailsDto> CreateAsync(CreateProductDto newProduct)
        {
            Product product = new()
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                CategoryId = newProduct.CategoryId,
                Price = newProduct.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _stockService.CreateOrUpdateStockAsync(product.Id, newProduct.Stock);
            var currentStock = await _stockService.GetStockByProductIdAsync(product.Id);

            return new ProductDetailsDto(
                product.Id,
                product.Name,
                product.Description ?? "",
                (int)product.CategoryId,
                product.Category != null ? product.Category.Name.ToString() : "",
                product.Price,
                currentStock,
                product.IsActive
            );
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _context.Products
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return false;
            }

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.CategoryId = updateDto.CategoryId;
            product.Price = updateDto.Price;
            product.IsActive = updateDto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _stockService.CreateOrUpdateStockAsync(product.Id, updateDto.Stock);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return false;
            }
            product.IsActive = false; //soft delete
            product.DeletedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
