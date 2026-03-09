using Loja.Dtos.Order;

namespace Loja.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDetailsDto>> GetAllAsync();

        Task<OrderDetailsDto?> GetByIdAsync(int id);

        Task<OrderDetailsDto> CreateAsync(CreateOrderDto createDto);

        Task<bool> UpdateStatusAsync(int id, UpdateStatusOrderDto updateDto);

        Task<bool> DeleteAsync(int id);
    }
}
