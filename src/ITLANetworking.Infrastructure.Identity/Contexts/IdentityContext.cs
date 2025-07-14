using ITLANetworking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ITLANetworking.Infrastructure.Identity.Contexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar el esquema de Identity
            modelBuilder.HasDefaultSchema("Identity");

            #region Identity Tables
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "Identity");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Identity");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");
            #endregion

            #region Property Configurations
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Phone)
                .HasMaxLength(20);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.ProfilePicture)
                .HasMaxLength(500);
            #endregion

            // Aplicar configuraciones del assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
