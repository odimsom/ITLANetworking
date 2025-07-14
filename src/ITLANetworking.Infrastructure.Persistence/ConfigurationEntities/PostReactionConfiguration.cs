using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITLANetworking.Infrastructure.Persistence.ConfigurationEntities
{
    public class PostReactionConfiguration : IEntityTypeConfiguration<PostReaction>
    {
        public void Configure(EntityTypeBuilder<PostReaction> builder)
        {
            builder.ToTable("PostReactions");

            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.Id)
                .ValueGeneratedOnAdd();

            builder.Property(pr => pr.UserId)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(pr => pr.PostId)
                .IsRequired();

            builder.Property(pr => pr.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(pr => pr.Created)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Unique constraint - un usuario solo puede tener una reacción por post
            builder.HasIndex(pr => new { pr.UserId, pr.PostId })
                .IsUnique()
                .HasDatabaseName("IX_PostReactions_UserPost");

            // Navigation properties
            builder.HasOne(pr => pr.User)
                .WithMany(u => u.PostReactions)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict

            builder.HasOne(pr => pr.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(pr => pr.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}