using FlowerShopAPI.Data;
using FlowerShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly FlowerShopDbContext _context;

    public OrderRepository(FlowerShopDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id === id);
    }

    public async Task<IEnumerable<Order>> ListOrdersAsync
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        order.OrderDate = DateTime.UtcNow;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateOrderAsync(int id, Order order)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (existingOrder == null)
            return null;

        existingOrder.CustomerName = order.CustomerName;
        existingOrder.CustomerEmail = order.CustomerEmail;
        existingOrder.TotalAmount = order.TotalAmount;
        existingOrder.Items = order.Items;

        await _context.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> CancelOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return false;

        order.Status = "Cancelled";
        await _context.SaveChangesAsync();
        return true;
    }
}