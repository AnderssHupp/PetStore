using Loja.Data;
using Loja.Dtos.Order;
using Loja.Models;
using Loja.Models.Enums;
using Loja.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Loja.Services
{
    public class OrderService(PetStoreContext context, IStockService stockService) : IOrderService
    {
        private readonly PetStoreContext _context = context;
        private readonly IStockService _stockService = stockService;

        public async Task<IEnumerable<OrderDetailsDto>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .Select(o => new OrderDetailsDto
                (
                    o.Id,
                    o.UserId,
                    (int)o.Status,
                    o.Status.ToString(),
                    o.Items.Sum(i => i.Quantity * i.PriceAtPurchase),
                    o.CreatedAt,
                    o.Items.Select(i => new OrderItemDetailsDto
                    (
                        i.ProductId,
                        i.Product != null ? i.Product.Name : "Unknown Product",
                        i.PriceAtPurchase,
                        i.Quantity,
                        i.PriceAtPurchase * i.Quantity
                    )).ToList()
                ))
                .ToListAsync();
        }

        public async Task<OrderDetailsDto?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .AsNoTracking()
                .Select(o => new OrderDetailsDto
                (
                    o.Id,
                    o.UserId,
                    (int)o.Status,
                    o.Status.ToString(),
                    o.Items.Sum(i => i.Quantity * i.PriceAtPurchase),
                    o.CreatedAt,
                    o.Items.Select(i => new OrderItemDetailsDto
                    (
                        i.ProductId,
                        i.Product != null ? i.Product.Name : "Unknown Product",
                        i.PriceAtPurchase,
                        i.Quantity,
                        i.PriceAtPurchase * i.Quantity
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

        }

        public async Task<OrderDetailsDto> CreateAsync(CreateOrderDto newOrder)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var (products, quantityByProduct) = await GetProductsAndQuantitiesAsync(newOrder);

                var user = await GetUserOrThrowAsync(newOrder.UserId);

                await ValidateStockAsync(products, quantityByProduct);

                var total = newOrder.Items.Sum(i => products[i.ProductId].Price * i.Quantity);


                await DecreaseStockAsync(quantityByProduct);

                var order = BuildOrderWithItems(newOrder, products, user.Id, total);

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToOrderDetailsDto(order, products, newOrder.Items);
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Conflito de concorrência detectado. Tente novamente.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<(Dictionary<int, Product> products, Dictionary<int, int> quantityByProduct)> GetProductsAndQuantitiesAsync(CreateOrderDto newOrder)
        {
            var productIds = newOrder.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach (var item in newOrder.Items)
            {
                if (!products.TryGetValue(item.ProductId, out _))
                    throw new ArgumentException($"Produto com Id {item.ProductId} não encontrado.");
            }

            var quantityByProduct = newOrder.Items
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

            return (products, quantityByProduct);
        }

        private async Task<User> GetUserOrThrowAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && u.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync()
                ?? throw new ArgumentException($"Usuário com Id {userId} não encontrado.");
        }

        private async Task ValidateStockAsync(Dictionary<int, Product> products, Dictionary<int, int> quantityByProduct)
        {
            foreach (var (productId, requestedQty) in quantityByProduct)
            {
                var available = await _stockService.GetStockByProductIdAsync(productId);
                if (available < requestedQty)
                {
                    var name = products[productId].Name ?? $"Id {productId}";
                    throw new InvalidOperationException(
                        $"Estoque insuficiente para o produto '{name}': solicitado {requestedQty}, disponível {available}.");
                }
            }
        }
        private Order BuildOrderWithItems(CreateOrderDto newOrder, Dictionary<int, Product> products, int userId, decimal total)
        {
            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                Total = total,
                CreatedAt = DateTime.UtcNow
            };
            foreach (var item in newOrder.Items)
            {
                var product = products[item.ProductId];
                order.AddItem(product, item.Quantity);
            }
            order.RecalculateTotal();
            return order;
        }

        private async Task DecreaseStockAsync(Dictionary<int, int> quantityByProduct)
        {
            foreach (var (productId, requestedQty) in quantityByProduct)
            {
                var decreaseStock = await _stockService.DecreaseStockAsync(productId, requestedQty);
                if (!decreaseStock)
                    throw new InvalidOperationException($"Estoque insuficiente para o produto Id {productId}.");
            }
        }

        private static OrderDetailsDto MapToOrderDetailsDto(Order order, Dictionary<int, Product> products, List<CreateOrderItemDto> items)
        {
            var itemDtos = items.Select(i =>
            {
                var p = products[i.ProductId];
                return new OrderItemDetailsDto(
                    i.ProductId,
                    p.Name ?? "Unknown Product",
                    p.Price,
                    i.Quantity,
                    p.Price * i.Quantity
                );
            }).ToList();

            return new OrderDetailsDto(
                order.Id,
                order.UserId,
                (int)order.Status,
                order.Status.ToString(),
                order.Total,
                order.CreatedAt,
                itemDtos
            );
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateStatusOrderDto updateDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;
            order.Status = updateDto.Status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.Id == id && o.DeletedAt == null)
            .FirstOrDefaultAsync();
            if (order == null) return false;
            order.DeletedAt = DateTime.UtcNow;
            //_context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
