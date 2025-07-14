using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITLANetworking.Infrastructure.Persistence.ConfigurationEntities
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships");
            
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Property(f => f.RequesterId)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(f => f.ReceiverId)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(f => f.Status)
                .IsRequired(false)
                .HasConversion<int>()
                .HasDefaultValue(FriendshipStatus.Pending);

            builder.Property(f => f.RequestDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(f => f.ResponseDate)
                .IsRequired(false);

            // Unique constraint
            builder.HasIndex(f => new { f.RequesterId, f.ReceiverId })
                .IsUnique()
                .HasDatabaseName("IX_Friendships_RequesterReceiver");

            // Navigation properties
            builder.HasOne(f => f.Requester)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
