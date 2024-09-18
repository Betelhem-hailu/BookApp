
namespace BookStore.Services.Interfaces {
public interface IAuthService
{
    User Authenticate(string email, string password);
    string GenerateToken(User user);
    bool RegisterUser(User user);
    void AssignRoleToUser(Guid userId, string roleName);
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    User GetByIdAsync(Guid id);
}
}