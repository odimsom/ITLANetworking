using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITLANetworking.Infrastructure.Persistence.ConfigurationEntities
{
    public class BattleshipGameConfiguration : IEntityTypeConfiguration<BattleshipGame>
    {
        public void Configure(EntityTypeBuilder<BattleshipGame> builder)
        {
            builder.ToTable("BattleshipGames");

            builder.HasKey(bg => bg.Id);

            builder.Property(bg => bg.Id)
                .ValueGeneratedOnAdd();

            builder.Property(bg => bg.Player1Id)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(bg => bg.Player2Id)
                .HasMaxLength(450)
                .IsRequired(false);

            builder.Property(bg => bg.CurrentPlayerId)
                .HasMaxLength(450)
                .IsRequired(false);

            builder.Property(bg => bg.WinnerId)
                .HasMaxLength(450)
                .IsRequired(false);

            builder.Property(bg => bg.Status)
                .HasConversion<int>()
                .HasDefaultValue(GameStatus.WaitingForPlayers)
                .IsRequired(false);


            builder.Property(bg => bg.StartDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(bg => bg.EndDate)
                .IsRequired(false);

            builder.Property(bg => bg.Player1ShipsConfigured)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(bg => bg.Player2ShipsConfigured)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships with Ships and Attacks
            builder.HasMany(bg => bg.Ships)
                .WithOne(bs => bs.Game)
                .HasForeignKey(bs => bs.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(bg => bg.Attacks)
                .WithOne(ba => ba.Game)
                .HasForeignKey(ba => ba.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship to CurrentPlayer (no inverse navigation in User)
            builder.HasOne(bg => bg.CurrentPlayer)
                .WithMany()
                .HasForeignKey(bg => bg.CurrentPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            // NOTE: Player1, Player2, and Winner are configured in UserConfiguration.cs
        }
    }
}
