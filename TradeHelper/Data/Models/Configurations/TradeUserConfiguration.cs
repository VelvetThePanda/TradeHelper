using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TradeHelper.Data.Models.Configurations;

public class TradeUserConfiguration : IEntityTypeConfiguration<TradeUser>
{
    public void Configure(EntityTypeBuilder<TradeUser> builder)
    {
        builder.HasKey(t => t.ID);
        
        builder.Property(t => t.ID)
            .ValueGeneratedNever();

        builder.Property(t => t.Reputation)
            .ValueGeneratedNever();
    }
}