using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Interactivity;
using Remora.Results;
using TradeHelper.Services;

namespace TradeHelper.Commands.Interactivity;

public class TradeCreateModal : IModalInteractiveEntity
{
    private readonly ITradeService _tradeService;
    private readonly InteractionContext _context;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public TradeCreateModal(ITradeService tradeService, InteractionContext context, IDiscordRestInteractionAPI interactions)
    {
        _tradeService = tradeService;
        _context      = context;
        _interactions = interactions;
    }

    public async Task<Result<bool>> IsInterestedAsync(ComponentType? componentType, string customID, CancellationToken ct)
    {
        return Result<bool>.FromSuccess(customID is "create-trade");
    }

    public async Task<Result> HandleInteractionAsync(IUser user, string customID, IReadOnlyList<IPartialMessageComponent> components, CancellationToken ct)
    {
        var want = ((IPartialTextInputComponent)((IPartialActionRowComponent)components[0]).Components.Value[0]).Value.Value;
        var give = ((IPartialTextInputComponent)((IPartialActionRowComponent)components[1]).Components.Value[0]).Value.Value;

        await _tradeService.CreateTradeOfferAysync(_context.GuildID.Value, user.ID, null, want, give);

        return Result.FromSuccess();
    }
}