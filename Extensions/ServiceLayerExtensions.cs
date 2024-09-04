using BookStore.Services;
using BookStore.Services.Interfaces;
using BookStore.Infrastructure;

namespace BookStore.Extensions {
public static class ServiceLayerExtensions
{
    public static void ConfigureBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ImageService>();
        // Add other services here
    }
}
}