using BookStore.Repositories;
using Microsoft.Extensions.DependencyInjection;
using BookStore.Repositories.Interfaces;


namespace BookStore.Extensions {
public static class RepositoryExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        // Add other repositories here
    }
}
}