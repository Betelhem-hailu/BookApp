public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ProfileImageUrl { get; set; }
    public Order Order { get; set; }
    public List<Role> Roles { get; set; } = new();
    public ICollection<Order> Orders { get; set; }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public void UpdateUser(User newUser)
    {
        Email = newUser.Email;
        Password = newUser.Password;
        Roles = newUser.Roles;
    }
    
    public void addRole(Role role)
    {
        Roles.Add(role);
    }
}