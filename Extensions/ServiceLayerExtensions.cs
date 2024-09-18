using BookStore.Services;
using BookStore.Services.Interfaces;
using BookStore.Infrastructure;
using BookStore.Data.Cart;

namespace BookStore.Extensions {
public static class ServiceLayerExtensions
{
    public static void ConfigureBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ImageService>();
        services.AddScoped<ShoppingCart>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IOrderService, OrderService>();
        // Add other services here
    }
}
}