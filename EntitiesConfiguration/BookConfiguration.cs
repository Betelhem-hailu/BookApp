using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookStore.Models; // Adjust the namespace to match your project

namespace BookStore.EntitiesConfiguration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // Set the primary key
            builder.HasKey(b => b.BookId);

            // Configure the Title property
            builder.Property(b => b.Title)
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(200)
                .IsRequired();
            
            builder.Property(b => b.Title)
                .HasColumnName("Author")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(200)
                .IsRequired();

            // Configure the Price property
            builder.Property(b => b.Price)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            // Configure the ISBN property
            builder.Property(b => b.ISBN)
                .HasColumnName("ISBN")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(13)
                .IsRequired();
            
            // Configure the CoverImageUrl property
            builder.Property(b => b.CoverImageUrl);

             // Configure the many-to-many relationship with Category
            builder.HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookCategory",  // The name of the join table
                    b => b.HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    c => c.HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
            
            // Configure the relationship with OrderItem
            builder
                .HasMany(b => b.OrderItems)
                .WithOne(oi => oi.Book)
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
