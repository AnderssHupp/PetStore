using Loja.Models;
using Loja.Data;
using Loja.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Loja.Services
{
    public class StockService(PetStoreContext context) : IStockService
    {

        private readonly PetStoreContext _context = context;

        public async Task<int> GetStockByProductIdAsync(int productId)
        {
            var stock = await _context.Stock
                .Where(s => s.ProductId == productId)
                .Select(s => s.Quantity)
                .FirstOrDefaultAsync();
            return stock;
        }

        public async Task<Stock> CreateOrUpdateStockAsync(int productId, int quantity)
        {
            var stock = await _context.Stock
                .Where(s => s.ProductId == productId)
                .FirstOrDefaultAsync();
            if (stock == null)
            {
                stock = new Stock
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Stock.Add(stock);
            }
            else
            {
                stock.IncreaseQuantity(quantity);
            }
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> DecreaseStockAsync(int productId, int quantity)
        {
            var stock = await _context.Stock
                .Where(s => s.ProductId == productId)
                .FirstOrDefaultAsync();
            if (stock == null || stock.Quantity < quantity)
            {
                return false; // Produto não encontrado ou estoque insuficiente
            }
            stock.DecreaseQuantity(quantity);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
