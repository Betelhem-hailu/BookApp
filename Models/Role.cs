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

// command for database migrate and update for list of migrations
// dotnet ef migrations add add_desc_lang_date_onBooks
//  dotnet ef database update <MigrationName>
// dotnet ef migrations list