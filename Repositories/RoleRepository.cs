using BookStore.Context;
using BookStore.Repositories.Interfaces;

public class RoleRepository : IRoleRepository
{
    private readonly BookDbContext _context;

    public RoleRepository(BookDbContext context)
    {
        _context = context;
    }

    public Role GetRoleByName(string roleName)
    {
        return _context.Roles.FirstOrDefault(r => r.Name == roleName);
    }

    public IEnumerable<Role> GetAllRoles()
    {
        return _context.Roles.ToList();
    }

    public void AddRole(Role role)
    {
        _context.Roles.Add(role);
        _context.SaveChanges();
    }
}
