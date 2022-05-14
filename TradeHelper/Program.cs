// See https://aka.ms/new-console-template for more information
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Remora.Commands.Extensions;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Services;
using Remora.Discord.Hosting.Extensions;
using TradeHelper.Commands;
using TradeHelper.Data;
using TradeHelper.Data.MediatR;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory());
        configuration.AddJsonFile("appSettings.json", true, false);
        configuration.AddUserSecrets("VelvetThePanda-TradeHelper", false);
    })
    .ConfigureServices((context, services) =>
    {
        services
            .AddMediatR(typeof(TradeContext))
            .AddDbContext<TradeContext>()
            .AddDiscordCommands(true)
            .AddCommandTree()
            .WithCommandGroup<TestCommand>()
            .Finish();
    })
    .AddDiscordService(s => s.GetRequiredService<IConfiguration>().GetConnectionString("Discord"))
    .ConfigureLogging(b => b.AddFilter("System.Net.*", LogLevel.Error))
    .UseConsoleLifetime()
    .Build();
    
host.Services.GetRequiredService<TradeContext>().Database.MigrateAsync();
host.Services.GetRequiredService<SlashService>().UpdateSlashCommandsAsync();

await host.RunAsync();