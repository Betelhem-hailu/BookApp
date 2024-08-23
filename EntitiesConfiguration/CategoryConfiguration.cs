using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookStore.Models;

namespace BookStore.EntitiesConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Set the primary key
            builder.HasKey(c => c.CategoryId);

            // Configure the Name property
            builder.Property(c => c.Name)
                .HasColumnName("CategoryName")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();
            
           // Configure the many-to-many relationship with Book
            builder.HasMany(c => c.Books)
                .WithMany(b => b.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "BookCategory",  // The name of the join table
                    b => b.HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade),
                    c => c.HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}
