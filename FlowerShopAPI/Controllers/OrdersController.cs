using FlowerShopAPI.Models;
using FlowerShopAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _orderRepository.GetOrderAsync(id);
        if (order == null)
            return NotFound();

        return order;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> ListOrders()
    {
        return Ok(await _orderRepository.ListOrdersAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        var createdOrder = await _orderRepository.CreateOrderAsync(order);
        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> UpdateOrder(int id, Order order)
    {
        var updatedOrder = await _orderRepository.UpdateOrderAsync(id, order);
        if (updatedOrder == null)

        return updatedOrder;
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var result = await _orderRepository.CancelOrderAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}