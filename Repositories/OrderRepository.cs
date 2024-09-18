using BookStore.Repositories.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using BookStore.Context;

namespace BookStore.Repositories
{
    public class OrderRepository : IOrderRepository
{
    private readonly BookDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(BookDbContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }



    public async Task<IEnumerable<Order>> GetOrderByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Book)
            .Where(o => o.UserId == userId)
            .ToListAsync(cancellationToken);

            return orders;
    }

    public async Task StoreOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task StoreOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken)
    {
        await _context.OrderItems.AddAsync(orderItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

}


}
