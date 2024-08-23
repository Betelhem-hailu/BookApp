public class Role 
{
    public int RoleId { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; } = new();

    public Role(string name)
    {
        Name = name;
    }
}