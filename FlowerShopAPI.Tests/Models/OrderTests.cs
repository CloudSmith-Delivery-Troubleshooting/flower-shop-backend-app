using FlowerShopAPI.Models;
using Xunit;

namespace FlowerShopAPI.Tests.Models;

public class OrderTests
{
    [Fact]
    public void Order_PropertiesInitializeCorrectly()
    {
        // Arrange & Act
        var order = new Order
        {
            Id = 1,
            CustomerName = "Test Customer",
            CustomerEmail = "test@example.com",
            OrderDate = new DateTime(2023, 1, 1),
            TotalAmount = 100.00m,
            Status = "Pending"
        };

        // Assert
        Assert.Equal(1, order.Id);
        Assert.Equal("Test Customer", order.CustomerName);
        Assert.Equal("test@example.com", order.CustomerEmail);
        Assert.Equal(new DateTime(2023, 1, 1), order.OrderDate);
        Assert.Equal(100.00m, order.TotalAmount);
        Assert.Equal("Pending", order.Status);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void OrderItem_PropertiesInitializeCorrectly()
    {
        // Arrange & Act
        var orderItem = new OrderItem
        {
            Id = 1,
            OrderId = 1,
            FlowerName = "Rose",
            Quantity = 5,
            UnitPrice = 10.00m
        };

        // Assert
        Assert.Equal(1, orderItem.Id);
        Assert.Equal(1, orderItem.OrderId);
        Assert.Equal("Rose", orderItem.FlowerName);
        Assert.Equal(5, orderItem.Quantity);
        Assert.Equal(10.00m, orderItem.UnitPrice);
    }

    [Fact]
    public void Order_CanAddItems()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            CustomerName = "Test Customer",
            CustomerEmail = "test@example.com",
            OrderDate = new DateTime(2023, 1, 1),
            TotalAmount = 150.00m,
            Status = "Pending"
        };

        // Act
        order.Items.Add(new OrderItem
        {
            Id = 1,
            OrderId = 1,
            FlowerName = "Rose",
            Quantity = 5,
            UnitPrice = 10.00m
        });

        order.Items.Add(new OrderItem
        {
            Id = 2,
            OrderId = 1,
            FlowerName = "Tulip",
            Quantity = 10,
            UnitPrice = 5.00m
        });

        // Assert
        Assert.Equal(2, order.Items.Count);
        Assert.Equal("Rose", order.Items[0].FlowerName);
        Assert.Equal("Tulip", order.Items[1].FlowerName);
    }
}