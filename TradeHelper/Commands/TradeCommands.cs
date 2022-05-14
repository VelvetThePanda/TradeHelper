using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Remora.Commands.Attributes;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace TradeHelper.Commands;

[Group("trade")]
public class TradeCommands
{
    private readonly InteractionContext _context;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public TradeCommands(InteractionContext context, IDiscordRestInteractionAPI interactions)
    {
        _context = context;
        _interactions = interactions;
    }
    
    [Ephemeral]
    [Command("claim")]
    [Description("Claims a trade offer")]
    public async Task<IResult> Claim
    (
        [Option("claim_id")]
        [Description("The trade to claim.")]
        string claimID
    )
    {
        return Result.FromSuccess();
    }
    
}