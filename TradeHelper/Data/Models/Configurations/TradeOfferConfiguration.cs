using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TradeHelper.Data.Models.Configurations;

public class TradeOfferConfiguration : IEntityTypeConfiguration<TradeOffer>
{
    public void Configure(EntityTypeBuilder<TradeOffer> builder)
    {
        builder.HasKey(t => t.ID);
        
        builder.Property(t => t.ID)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.CreatedAt)
            .ValueGeneratedOnAdd();

        builder.HasOne(t => t.Owner)
            .WithMany(t => t.TradeOffers)
            .HasForeignKey(t => t.OwnerID);

        builder.HasOne(t => t.Claimer)
            .WithMany(t => t.ClaimedTrades)
            .HasForeignKey(t => t.ClaimerID);
    }
}