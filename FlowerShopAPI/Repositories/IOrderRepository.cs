using FlowerShopAPI.Models;

namespace FlowerShopAPI.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetOrderAsync(int id);
    Task<IEnumerable<Order>> ListOrdersAsync();
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> UpdateOrderAsync(int id, Order order);
    Task<bool> CancelOrderAsync(int id);
}