using System.Drawing;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TradeHelper.Data.MediatR.Users;
using TradeHelper.Services;

namespace TradeHelper.Commands;


public class UserCommands : CommandGroup
{
    private readonly Color[] _repColors = new[]
    {
        Color.FromArgb(168, 12, 12),
        Color.FromArgb(255, 57, 18),
        Color.FromArgb(255, 106, 20),
        Color.FromArgb(250, 152, 40),
        Color.FromArgb(255, 240, 25),
        Color.FromArgb(234, 252, 40),
        Color.FromArgb(192, 252, 40),
        Color.FromArgb(45, 92, 11),
        Color.FromArgb(103, 214, 159),
        Color.FromArgb(67, 232, 111),
        Color.FromArgb(30, 250, 59),
    };
    
    private readonly IMediator _mediator;
    private readonly ITradeService _trades;
    private readonly InteractionContext _context;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public UserCommands(IMediator mediator, ITradeService trades, InteractionContext context, IDiscordRestInteractionAPI interactions)
    {
        _mediator = mediator;
        _trades = trades;
        _context = context;
        _interactions = interactions;
    }

    [Ephemeral]
    [Command("Trader Profile")]
    [CommandType(ApplicationCommandType.User)]
    public async Task<IResult> ProfileAsync()
    {
        var resolvedUser = _context.Data.Resolved.Value.Users.Value.First();
        
        var user = await _mediator.Send(new GetUser.Request(resolvedUser.Key));
        
        if (user is null)
            return await _interactions.EditOriginalInteractionResponseAsync
            (
                _context.ApplicationID, 
                _context.Token,
                "That user hasn't had any trades, and thus doesn't have a profile."
            );

        var firstTrade = user.TradeOffers.FirstOrDefault() ?? user.ClaimedTrades.FirstOrDefault();
        
        // Needlessly complex algorithm to get map a color to a reputation. Safe to ignore.
        var embedColor = _repColors[Math.Clamp((int)((user.Reputation + _repColors.Length / 2) / (int)(Math.Max(user.TradeOffers.Count + user.ClaimedTrades.Count / 2, 4) / 4)) + 2, 0, _repColors.Length - 1)];
        var averageTradeCompletionTime = user.ClaimedTrades.Count is 0 ? 0 : (int)user.ClaimedTrades.Select(t => (t.CompletedAt - t.CreatedAt ?? default(TimeSpan)).TotalMinutes).Average();
        
        var embed = new Embed
        {
            Title = "Trader Profile",
            Thumbnail = _context.User.Avatar is null
                ? new(CDN.GetDefaultUserAvatarUrl(resolvedUser.Value, imageSize: 512).Entity.ToString())
                : new EmbedThumbnail(CDN.GetUserAvatarUrl(resolvedUser.Value, imageSize: 512).Entity.ToString()),
            Colour = embedColor,
            Fields = new EmbedField[]
            {
                new("Trading Since", firstTrade?.CreatedAt is {} dto ? $"<t:{((DateTimeOffset)dto).ToUnixTimeSeconds()}:F>" : "None"),
                new("Reputation", user.Reputation is 0 ? "None" : $"{user.Reputation}"),
                new("Trades", user.TradeOffers.Count is 0 ? "None" : $"{user.TradeOffers.Count}"),
                new("Claimed Trades", user.ClaimedTrades.Count is 0 ? "None" : $"{user.ClaimedTrades.Count}"),
                new("Average Trade Duration", averageTradeCompletionTime is 0 ? "N/A" : $"~{averageTradeCompletionTime} minutes")
            }
        };
        
        return await _interactions.EditOriginalInteractionResponseAsync
        (
            _context.ApplicationID, 
            _context.Token,
            embeds: new[] { embed }
        );

    }

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