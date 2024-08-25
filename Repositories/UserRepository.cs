using BookStore.Context;
using Microsoft.EntityFrameworkCore;
using BookStore.Repositories.Interfaces;

namespace BookStore.Repositories {
public class UserRepository : IUserRepository
{
    private readonly BookDbContext _context;

    public UserRepository(BookDbContext context)
    {
        _context = context;
    }

    public bool UserExists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }


    public User GetUserByEmail(string email)
    {
        return _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Email == email);
    }

    public User GetUserById(Guid userId)
    {
        return _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.UserId == userId);
    }


    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
    public IEnumerable<Role> GetUserRoles(Guid userId)
    {
        return _context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Roles)
            .ToList();
    }
}
}