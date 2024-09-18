using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookStore.Models; // Adjust the namespace to match your project

namespace BookStore.EntitiesConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Set the primary key
            builder.HasKey(o => o.OrderId);

            // set the order date
            builder.Property(o => o.OrderDate)
                .IsRequired();

               // Configure the relationship with User
            builder
                .HasOne<User>() // Specify that an Order has one User
                .WithMany(u => u.Orders) // A User can have many Orders
                .HasForeignKey(o => o.UserId) // Use UserId as the foreign key
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship with OrderItem
            builder
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
