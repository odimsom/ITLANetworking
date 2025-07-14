using ITLANetworking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITLANetworking.Infrastructure.Persistence.ConfigurationEntities
{
    public class BattleshipAttackConfiguration : IEntityTypeConfiguration<BattleshipAttack>
    {
        public void Configure(EntityTypeBuilder<BattleshipAttack> builder)
        {
            builder.ToTable("BattleshipAttacks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.GameId)
                .IsRequired();

            builder.Property(x => x.AttackerId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.Row)
                .IsRequired()
                .HasComment("Attack row position (0-9)");

            builder.Property(x => x.Column)
                .IsRequired()
                .HasComment("Attack column position (0-9)");

            builder.Property(x => x.IsHit)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.AttackDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Unique constraint - no duplicate attacks at same position
            builder.HasIndex(x => new { x.GameId, x.Row, x.Column })
                .IsUnique()
                .HasDatabaseName("IX_BattleshipAttacks_GameRowColumn");

            // Relationships
            builder.HasOne(x => x.Game)
                .WithMany(x => x.Attacks)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Attacker)
                .WithMany()
                .HasForeignKey(x => x.AttackerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Constraints

            builder.HasCheckConstraint("CK_BattleshipAttacks_Row", "Row >= 0 AND Row <= 9");
            builder.HasCheckConstraint("CK_BattleshipAttacks_Column", "[Column] >= 0 AND [Column] <= 9");
        }
    }
}
