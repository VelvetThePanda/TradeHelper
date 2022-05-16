using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TradeHelper.Services;

namespace TradeHelper.Commands;

public class UserCommands : CommandGroup
{
    private readonly ITradeService _trades;
    private readonly InteractionContext _context;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public UserCommands(ITradeService trades, InteractionContext context, IDiscordRestInteractionAPI interactions)
    {
        _trades = trades;
        _context = context;
        _interactions = interactions;
    }
    
    [Command("Trader Profile")]
    [CommandType(ApplicationCommandType.User)]
    public async Task<IResult> ProfileAsync() => throw new NotImplementedException();
    
    [Command("+Rep")]
    [CommandType(ApplicationCommandType.User)]
    public async Task<IResult> RepAsync() => throw new NotImplementedException();

    
    [Command("-Rep")]
    [CommandType(ApplicationCommandType.User)]
    public async Task<IResult> UnRepAsync() => throw new NotImplementedException();
    
}