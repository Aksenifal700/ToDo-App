using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.DataAccess.Database.Entities;

namespace TodoApp.DataAccess.Database.EntityConfiguration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{

    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.HasKey(c => c.Id);
        
        entity.Property(c => c.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        entity.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        entity.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        entity.ToTable("Category");
        
        entity.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}