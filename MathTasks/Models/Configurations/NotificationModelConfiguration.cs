using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MathTasks.Models.Configurations
{
    public class NotificationModelConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Nofifications");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.UpdatedBy).IsRequired();

            builder.Property(x => x.Subject).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(3000);
            builder.Property(x => x.AddressFrom).IsRequired().HasMaxLength(256);
            builder.Property(x => x.AddressTo).IsRequired().HasMaxLength(256);
            builder.Property(x => x.IsCompleted);
        }
    }
}
