using Loja.Data;
using Loja.Models;
using Loja.Dtos.Cart;
using Loja.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Loja.Services
{
    public class CartService : ICartService
    {
        private readonly PetStoreContext _context;
        public CartService(PetStoreContext context)
        {
            _context = context;
        }

        public async Task<CartDetailsDto> GetOrCreateCartBySessionIdAsync(string sessionId)
        {
            var cart = await GetCartEntityBySessionIdAsync(sessionId);

            if (cart == null)
            {
                cart = new Cart
                {
                    SessionId = sessionId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return MapToDetailsDto(cart);
        }

        public async Task<CartDetailsDto?> GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .Where(c => c.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return cart != null ? MapToDetailsDto(cart) : null;
        }

        public async Task<CartDetailsDto> AddItemsToCartAsync(string sessionId, List<CreateCartItemDto> items)
        {
            var cart = await GetCartEntityBySessionIdAsync(sessionId);

            if (cart == null)
            {
                cart = new Cart
                {
                    SessionId = sessionId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Buscar todos os produtos de uma vez (otimização)

            var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach (var item in items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                {
                    throw new Exception($"Product {item.ProductId} not found");
                }

                // Verificar se o item já existe no carrinho
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (existingItem != null)
                {
                    // Se já existe, incrementa a quantidade
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    // Se não existe, cria novo item
                    cart.Items.Add(new CartItem
                    {
                        Cart = cart,
                        CartId = cart.Id,
                        ProductId = item.ProductId,
                        Product = product,
                        Quantity = item.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return MapToDetailsDto(cart);
        }

        public async Task<CartDetailsDto?> RemoveItemsFromCartAsync(string sessionId, List<int> productIds)
        {
            var cart = await GetCartEntityBySessionIdAsync(sessionId);
            if (cart == null)
                return null;

            // Buscar itens do carrinho que correspondem aos productIds
            var itemsToRemove = cart.Items
                .Where(i => productIds.Contains(i.ProductId))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                _context.CartItems.Remove(item);
            }

            await _context.SaveChangesAsync();
            return MapToDetailsDto(cart);
        }

        public async Task<CartDetailsDto?> UpdateCartItemQuantityAsync(string sessionId, int productId, int quantity)
        {
            var cart = await GetCartEntityBySessionIdAsync(sessionId);
            if (cart == null)
                return null;

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
                return null;

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return MapToDetailsDto(cart);
        }

        public async Task<CartDetailsDto?> ClearCartAsync(string sessionId)
        {
            var cart = await GetCartEntityBySessionIdAsync(sessionId);
            if (cart == null)
                return null;

            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            return MapToDetailsDto(cart);
        }

        public async Task<CartDetailsDto?> GetCartBySessionIdAsync(string sessionId)
        {
            var cart = await GetCartEntityBySessionIdAsync(sessionId);
            return cart != null ? MapToDetailsDto(cart) : null;
        }

        // Método auxiliar para buscar entidade Cart 
        private async Task<Cart?> GetCartEntityBySessionIdAsync(string sessionId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        // Método auxiliar para mapear Cart para CartDetailsDto
        private CartDetailsDto MapToDetailsDto(Cart cart)
        {
            return new CartDetailsDto(
                cart.Id,
                cart.UserId,
                cart.Items.Select(i => new CartItemDetailsDto(
                    i.ProductId,
                    i.Product != null ? i.Product.Name : "Unknown Product",
                    i.Product != null ? i.Product.Price : 0m,
                    i.Quantity
                )).ToList(),
                cart.Items.Sum(i => (i.Product != null ? i.Product.Price : 0m) * i.Quantity)
            );
        }
    }
}
