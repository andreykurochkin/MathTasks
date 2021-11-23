using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MathTasks.Models.Configurations
{
    public class MathTaskModelConfiguration : IEntityTypeConfiguration<MathTask>
    {
        public void Configure(EntityTypeBuilder<MathTask> builder)
        {
            builder.ToTable("MathTasks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.UpdatedBy).IsRequired();

            builder.Property(x => x.Theme)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Content)
                .HasMaxLength(3000)
                .IsRequired();
            builder.HasMany(x => x.Tags)
                .WithMany(x => x.MathTasks);
        }
    }
}
