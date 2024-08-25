
namespace BookStore.Repositories.Interfaces{
public interface IRoleRepository
{
    Role GetRoleByName(string roleName);
    IEnumerable<Role> GetAllRoles();
    void AddRole(Role role);
}
}