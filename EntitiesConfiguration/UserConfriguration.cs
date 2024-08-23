using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookStore.Models;

namespace BookStore.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.Email)
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasColumnName("Password")
                .IsRequired();
        
            // Configure the ProfileImageUrl property
            builder.Property(x => x.ProfileImageUrl)
                .HasColumnType("NVARCHAR");

            // Configure the one-to-many relationship with Orders
        // builder
        //     .HasMany(x => x.Orders)
        //     .WithMany(x => x.Users)
        //     .UsingEntity<Dictionary<string, object>>(
        //         "OrderItem",
        //         order => order
        //             .HasOne<Order>()
        //             .WithMany()
        //             .HasForeignKey("OrderId")
        //             .OnDelete(DeleteBehavior.Cascade),
        //         user => user
        //             .HasOne<User>()
        //             .WithMany()
        //             .HasForeignKey("UserId")
        //             .OnDelete(DeleteBehavior.Cascade));

            builder
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    role => role
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    user => user
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}