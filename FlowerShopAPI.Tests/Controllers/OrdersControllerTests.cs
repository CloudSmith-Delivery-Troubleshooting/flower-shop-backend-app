using FlowerShopAPI.Controllers;
using FlowerShopAPI.Models;
using FlowerShopAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlowerShopAPI.Tests.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<IOrderRepository> _mockRepo;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _mockRepo = new Mock<IOrderRepository>();
        _controller = new OrdersController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetOrder_ReturnsOrder_WhenOrderExists()
    {
        // Arrange
        var testOrder = new Order { Id = 1, CustomerName = "Test Customer" };
        _mockRepo.Setup(repo => repo.GetOrderAsync(1)).ReturnsAsync(testOrder);

        // Act
        var result = await _controller.GetOrder(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        var returnValue = Assert.IsType<Order>(actionResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetOrderAsync(1)).ReturnsAsync((Order?)null);

        // Act
        var result = await _controller.GetOrder(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task ListOrders_ReturnsAllOrders()
    {
        // Arrange
        var testOrders = new List<Order>
        {
            new Order { Id = 1, CustomerName = "Customer 1" },
            new Order { Id = 2, CustomerName = "Customer 2" }
        };
        _mockRepo.Setup(repo => repo.ListOrdersAsync()).ReturnsAsync(testOrders);

        // Act
        var result = await _controller.ListOrders();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Order>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreatedAtAction()
    {
        // Arrange
        var orderToCreate = new Order { CustomerName = "New Customer" };
        var createdOrder = new Order { Id = 1, CustomerName = "New Customer" };
        
        _mockRepo.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
            .ReturnsAsync(createdOrder);

        // Act
        var result = await _controller.CreateOrder(orderToCreate);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal("GetOrder", createdAtActionResult.ActionName);
        Assert.Equal(1, createdAtActionResult.RouteValues?["id"]);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsUpdatedOrder_WhenOrderExists()
    {
        // Arrange
        var orderToUpdate = new Order { Id = 1, CustomerName = "Updated Customer" };
        _mockRepo.Setup(repo => repo.UpdateOrderAsync(1, It.IsAny<Order>()))
            .ReturnsAsync(orderToUpdate);

        // Act
        var result = await _controller.UpdateOrder(1, orderToUpdate);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        var returnValue = Assert.IsType<Order>(actionResult.Value);
        Assert.Equal("Updated Customer", returnValue.CustomerName);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderToUpdate = new Order { Id = 1, CustomerName = "Updated Customer" };
        _mockRepo.Setup(repo => repo.UpdateOrderAsync(1, It.IsAny<Order>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _controller.UpdateOrder(1, orderToUpdate);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task CancelOrder_ReturnsNoContent_WhenOrderExists()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.CancelOrderAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.CancelOrder(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CancelOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.CancelOrderAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.CancelOrder(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}