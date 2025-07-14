using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITLANetworking.Infrastructure.Persistence.ConfigurationEntities
{
    public class BattleshipShipConfiguration : IEntityTypeConfiguration<BattleshipShip>
    {
        public void Configure(EntityTypeBuilder<BattleshipShip> builder)
        {
            builder.ToTable("BattleshipShips");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.GameId)
                .IsRequired();

            builder.Property(x => x.PlayerId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.Size)
                .IsRequired()
                .HasComment("Ship size: 2-5 cells");

            builder.Property(x => x.StartRow)
                .IsRequired()
                .HasComment("Starting row position (0-9)");

            builder.Property(x => x.StartColumn)
                .IsRequired()
                .HasComment("Starting column position (0-9)");

            builder.Property(x => x.Direction)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.IsSunk)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(x => x.Game)
                .WithMany(x => x.Ships)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Player)
                .WithMany()
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Constraints
            builder.HasCheckConstraint("CK_BattleshipShips_Size", "Size >= 2 AND Size <= 5");
            builder.HasCheckConstraint("CK_BattleshipShips_StartRow", "StartRow >= 0 AND StartRow <= 9");
            builder.HasCheckConstraint("CK_BattleshipShips_StartColumn", "StartColumn >= 0 AND StartColumn <= 9");
        }
    }
}
