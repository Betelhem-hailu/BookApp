using BookStore.Models;

namespace BookStore.Repositories.Interfaces {
public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrderByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task StoreOrderAsync(Order order, CancellationToken cancellationToken);
    Task StoreOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken);
}
}