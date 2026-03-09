using Loja.Models;
namespace Loja.Services.Interfaces
{
    public interface IStockService
    {
        Task<int> GetStockByProductIdAsync(int productId);
        Task<Stock> CreateOrUpdateStockAsync(int productId, int quantity);
        Task<bool> DecreaseStockAsync(int productId, int quantity);
    }
}
