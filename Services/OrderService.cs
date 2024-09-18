using Microsoft.EntityFrameworkCore;
using BookStore.Context;
using BookStore.Services.Interfaces;
using BookStore.Repositories.Interfaces;
using BookStore.Models;
using BookStore.HandleResponse;
using BookStore.DTOs;
using AutoMapper;

namespace BookStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response> GetOrderByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
    {
        var orders = await _orderRepository.GetOrderByUserAsync(userId,cancellationToken);
        var orderDtos = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);

        return new Response("Orders retrieved successfully", 200, orderDtos);
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        // _logger.LogError(ex, "An error occurred while retrieving books.");

        return new Response($"An error occurred: {ex.Message}", 500);
    }

    }

    public async Task<Response> StoreOrderAsync(List<ShoppingCartItem> items, Guid userId, string userEmail, CancellationToken cancellationToken)
    {
        var order = new Order()
        {
            UserId = userId,
            Email = userEmail,
            OrderDate =  DateTime.UtcNow
        };

        try
        {
        await _orderRepository.StoreOrderAsync(order, cancellationToken);
                
        foreach (var item in items)
        {
            var OrderItem = new OrderItem()
            {
                OrderId = order.OrderId,
                BookId = item.Book.BookId,
                Quantity = item.Quantity,
                UnitPrice = item.Book.Price
            };

                await _orderRepository.StoreOrderItemAsync(OrderItem, cancellationToken);
            }

        return new Response("Order created", 200);
        }

        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while ordering.");
            return new Response("Internal Server Error", 500);
        }
    }

    }
}