using Loja.Dtos.Cart;

namespace Loja.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDetailsDto?> GetOrCreateCartBySessionIdAsync(string sessionId);
        Task<CartDetailsDto?> AddItemsToCartAsync(string sessionId, List<CreateCartItemDto> items);
        Task<CartDetailsDto?> RemoveItemsFromCartAsync(string sessionId, List<int> productIds);
        Task<CartDetailsDto?> UpdateCartItemQuantityAsync(string sessionId, int productId, int quantity);
        Task<CartDetailsDto?> ClearCartAsync(string sessionId);
        Task<CartDetailsDto?> GetCartBySessionIdAsync(string sessionId);
    }
}