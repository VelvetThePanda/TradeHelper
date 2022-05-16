using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Interactivity;
using Remora.Rest.Core;
using Remora.Results;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;
using TradeHelper.Infrastructure;
using TradeHelper.Services;

namespace TradeHelper.Commands.Interactivity;

public class TradeListButtons : IButtonInteractiveEntity
{
    private static readonly Regex _regex = new(@"^trade-list-(?<type>\S{4,6})-page(?<page>\d+)-(?<ID>\d+)?$");

    private readonly ITradeService _trades;
    private readonly InteractionContext _context;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public TradeListButtons(ITradeService trades, InteractionContext context, IDiscordRestInteractionAPI interactions)
    {
        _trades = trades;
        _context = context;
        _interactions = interactions;
    }
    
    public async Task<Result<bool>> IsInterestedAsync(ComponentType? componentType, string customID, CancellationToken ct)
        => Result<bool>.FromSuccess(_regex.IsMatch(customID));

    public async Task<Result> HandleInteractionAsync(IUser user, string customID, CancellationToken ct)
    {
        var match = _regex.Match(customID);
        
        var id = match.Groups["ID"].Value;
        var type = match.Groups["type"].Value;
        var page = int.Parse(match.Groups["page"].Value);

        _ = Snowflake.TryParse(id, out var sn, Constants.DiscordEpoch);

        
        var tradeResult = 
            type is "global" 
                ? await _trades.GetTradeOffersAsync(_context.GuildID.Value, null) 
                : type is "self" 
                    ? await _trades.GetTradeOffersAsync(_context.GuildID.Value, user.ID) 
                    : await _trades.GetTradeOffersAsync(_context.GuildID.Value, sn);

        if (!tradeResult.IsDefined(out var trades))
        {
            var responseResult = await _interactions.EditOriginalInteractionResponseAsync
                (
                    _context.ApplicationID,
                    _context.Token,
                    tradeResult.Error!.Message
                );

            return Result.FromSuccess();
        }

        TradePaginationHelper.GetEmbedsAndComponents(trades, page, type, sn, out IReadOnlyList<IEmbed> embeds, out IReadOnlyList<IMessageComponent> components);

        await _interactions.EditOriginalInteractionResponseAsync(_context.ApplicationID, _context.Token, embeds: new(embeds), components: new(components));

        return Result.FromSuccess();
    }
    
}