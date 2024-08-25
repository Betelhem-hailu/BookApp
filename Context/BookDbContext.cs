using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BookStore.Models;

namespace BookStore.Context
{
    //BookDbContext class inherits from DbContext
    public class BookDbContext: DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role("Admin") { RoleId = 1 },
                new Role("User") { RoleId = 2 }
            );
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
