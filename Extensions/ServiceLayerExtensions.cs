using BookStore.Services;
using BookStore.Services.Interfaces;

namespace BookStore.Extensions {
public static class ServiceLayerExtensions
{
    public static void ConfigureBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        // Add other services here
    }
}
}