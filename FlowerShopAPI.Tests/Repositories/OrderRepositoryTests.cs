using FlowerShopAPI.Data;
using FlowerShopAPI.Models;
using FlowerShopAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FlowerShopAPI.Tests.Repositories;

public class OrderRepositoryTests
{
    private readonly DbContextOptions<FlowerShopDbContext> _options;

    public OrderRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<FlowerShopDbContext>()
            .UseInMemoryDatabase(databaseName: "FlowerShopTestDb")
            .Options;
    }

    private FlowerShopDbContext CreateContext()
    {
        var context = new FlowerShopDbContext(_options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task GetOrderAsync_ReturnsOrder_WhenOrderExists()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);
        var order = new Order
        {
            CustomerName = "Test Customer",
            CustomerEmail = "test@example.com",
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100.00m,
            Status = "Pending"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetOrderAsync(order.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.CustomerName);
    }

    [Fact]
    public async Task GetOrderAsync_ReturnsNull_WhenOrderDoesNotExist()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetOrderAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ListOrdersAsync_ReturnsAllOrders()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);

        var orders = new List<Order>
        {
            new Order { CustomerName = "Customer 1", CustomerEmail = "customer1@example.com", OrderDate = DateTime.UtcNow, TotalAmount = 100.00m },
            new Order { CustomerName = "Customer 2", CustomerEmail = "customer2@example.com", OrderDate = DateTime.UtcNow, TotalAmount = 200.00m }
        };

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ListOrdersAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateOrderAsync_CreatesNewOrder()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);
        var order = new Order
        {
            CustomerName = "New Customer",
            CustomerEmail = "new@example.com",
            TotalAmount = 150.00m,
            Items = new List<OrderItem>
            {
                new OrderItem { FlowerName = "Rose", Quantity = 5, UnitPrice = 10.00m },
                new OrderItem { FlowerName = "Tulip", Quantity = 10, UnitPrice = 5.00m }
            }
        };

        // Act
        var result = await repository.CreateOrderAsync(order);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.Equal("New Customer", result.CustomerName);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task UpdateOrderAsync_UpdatesExistingOrder()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);

        var order = new Order
        {
            CustomerName = "Original Customer",
            CustomerEmail = "original@example.com",
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100.00m,
            Status = "Pending"
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var updatedOrder = new Order
        {
            Id = order.Id,
            CustomerName = "Updated Customer",
            CustomerEmail = "updated@example.com",
            TotalAmount = 200.00m,
            Status = "Pending",
            Items = new List<OrderItem>()
        };

        // Act
        var result = await repository.UpdateOrderAsync(order.Id, updatedOrder);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Customer", result.CustomerName);
        Assert.Equal("updated@example.com", result.CustomerEmail);
        Assert.Equal(200.00m, result.TotalAmount);
    }

    [Fact]
    public async Task CancelOrderAsync_CancelsExistingOrder()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);

        var order = new Order
        {
            CustomerName = "Test Customer",
            CustomerEmail = "test@example.com",
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100.00m,
            Status = "Pending"
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.CancelOrderAsync(order.Id);
        var cancelledOrder = await context.Orders.FindAsync(order.Id);

        // Assert
        Assert.True(result);
        Assert.Equal("Cancelled", cancelledOrder?.Status);
    }

    [Fact]
    public async Task CancelOrderAsync_ReturnsFalse_WhenOrderDoesNotExist()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.CancelOrderAsync(999);

        // Assert
        Assert.False(result);
    }
}