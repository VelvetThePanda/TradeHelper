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

    [Ephemeral]
    [Command("+Rep")]
    [CommandType(ApplicationCommandType.User)]
    public async Task<IResult> RepAsync()
    {
        var repResult = await _trades.RepUserAsync(_context.User.ID, _context.Data.Resolved.Value.Users.Value.First().Key, true);

        if (!repResult.IsSuccess)
            return await _interactions.CreateFollowupMessageAsync
            (
                _context.ApplicationID,
                _context.Token,
                repResult.Error.Message,
                flags: MessageFlags.Ephemeral
            );
        
        return await _interactions.CreateFollowupMessageAsync
        (
            _context.ApplicationID,
            _context.Token,
            "Reputation added.",
            flags: MessageFlags.Ephemeral
        );
    }

    [Ephemeral]
    [Command("-Rep")]
    [CommandType(ApplicationCommandType.User)]
    public async Task<IResult> UnRepAsync()
    {
        var repResult = await _trades.RepUserAsync(_context.User.ID, _context.Data.Resolved.Value.Users.Value.First().Key, false);

        if (!repResult.IsSuccess)
            return await _interactions.CreateFollowupMessageAsync
            (
                _context.ApplicationID,
                _context.Token,
                repResult.Error.Message
            );
        
        return await _interactions.CreateFollowupMessageAsync
        (
            _context.ApplicationID,
            _context.Token,
            "Reputation removed."
        );
    }
    
}