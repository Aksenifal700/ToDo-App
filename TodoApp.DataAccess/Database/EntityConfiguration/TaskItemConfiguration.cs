using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.DataAccess.Database.Entities;

namespace TodoApp.DataAccess.Database.EntityConfiguration;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{

        public void Configure(EntityTypeBuilder<TaskItem> entity)
        {
                entity.HasKey(t => t.Id);
                
                entity.Property(t => t.Id)
                        .HasDefaultValueSql("gen_random_uuid()");
                
                entity.Property(t => t.Title)
                        .IsRequired()
                        .HasMaxLength(100);
                
                entity.Property(t => t.Description)
                        .IsRequired(false)
                        .HasMaxLength(1000);
                
                entity.Property(t => t.IsCompleted)
                        .IsRequired()
                        .HasDefaultValue(false);

                entity.Property(t => t.DueDate)
                        .IsRequired(false);
                
                entity.Property(t => t.CreatedAt)
                        .IsRequired()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(t => t.UpdatedAt)
                        .IsRequired(false)
                        .HasColumnType("timestamp without time zone");
                
                entity.Property(t => t.CategoryId)
                        .IsRequired(false);
                
                entity.ToTable("TaskItem");
                
                entity.HasOne(t => t.User)
                        .WithMany(t => t.TaskItems)
                        .HasForeignKey(t => t.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
}