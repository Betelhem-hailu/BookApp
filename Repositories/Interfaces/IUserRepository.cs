
namespace BookStore.Repositories.Interfaces {
public interface IUserRepository
{
    User GetUserByEmail(string email);
    User GetUserById(Guid userId);
    bool UserExists(string email);
    void AddUser(User user);
    void UpdateUser(User user);
    IEnumerable<Role> GetUserRoles(Guid userId);
}
}