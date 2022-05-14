using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TradeHelper.Data;

public class TradeContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("FileName=TradeHelper.db", options => options.MigrationsAssembly(typeof(TradeContext).Assembly.FullName));
        base.OnConfiguring(optionsBuilder);
    }
}