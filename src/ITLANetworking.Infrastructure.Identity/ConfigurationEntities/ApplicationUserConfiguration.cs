using ITLANetworking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITLANetworking.Infrastructure.Identity.ConfigurationEntities
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users", "Identity");

            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            builder.Property(u => u.ProfilePicture)
                .HasMaxLength(500);

            builder.Property(u => u.IsActive)
                .IsRequired();

            builder.Property(u => u.CreatedDate)
                .IsRequired();
        }
    }
}
