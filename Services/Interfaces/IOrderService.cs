using BookStore.HandleResponse;
using BookStore.Contracts;

namespace BookStore.Services.Interfaces {
public interface IOrderService
{
        Task<Response> StoreOrderAsync(List<ShoppingCartItem> items, Guid userId, string userEmail, CancellationToken cancellationToken);
        Task<Response> GetOrderByUserAsync(Guid userId, CancellationToken cancellationToken);    
}
}