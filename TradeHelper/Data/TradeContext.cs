using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using TradeHelper.Data.Models.Converters;

namespace TradeHelper.Data;

public class TradeContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("FileName=TradeHelper.db", options => options.MigrationsAssembly(typeof(TradeContext).Assembly.FullName));
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(TradeContext).Assembly);
        
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<Snowflake>().HaveConversion(typeof(SnowflakeConverter));
        builder.Properties<Snowflake?>().HaveConversion(typeof(NullableSnowflakeConverter));
    }
}