using Loja.Data;
using Loja.Dtos.Product;
using Loja.Models;
using Loja.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Loja.Services
{
    public class ProductService : IProductService
    {
        private readonly PetStoreContext _context;

        public ProductService(PetStoreContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<ProductDetailsDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .Select(p => new ProductDetailsDto(
                    p.Id,
                    p.Name,
                    p.Description ?? "",
                    p.CategoryId,
                    p.Category != null ? p.Category.Name : "",
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
                    p.CategoryId,
                    p.Category != null ? p.Category.Name : "",
                    p.Price,
                    p.Stock != null ? p.Stock.Quantity : 0,
                    p.IsActive
                ))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto newProduct)
        {
            Product product = new()
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                CategoryId = newProduct.CategoryId,
                Price = newProduct.Price,
                Stock = new Stock { Quantity = newProduct.Stock, CreatedAt = DateTime.UtcNow },
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.CategoryId,
                product.Price,
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

            if (product.Stock != null)
            {
                product.Stock.Quantity = updateDto.Stock;
                product.Stock.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                product.Stock = new Stock { Quantity = updateDto.Stock, Product = product };
            }

            await _context.SaveChangesAsync();
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
