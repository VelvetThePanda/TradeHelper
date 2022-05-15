using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace TradeHelper.Commands;

[Group("trade")]
public class TradeCommands : CommandGroup
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

    [Command("create")]
    [SuppressInteractionResponse(true)]
    [Description("Creates a trade offer")]
    public async Task<IResult> CreateAsync()
    {
        var data = new InteractionModalCallbackData
        (
            "create-trade",
            "Trade Details",
            new []
            {
                new ActionRowComponent
                (
                    new[]
                    {
                        new TextInputComponent("trade-receive", TextInputStyle.Short, "Trading For (Requesting)", default, 50, default, default, "What do you want?"),
                    }
                ),
                new ActionRowComponent
                (
                    new[]
                    {
                        new TextInputComponent("trade-offer", TextInputStyle.Short, "Trading For (Offering)", default, 50, default, default, "What do you offer?"),
                    }
                ),
            }
        );

        var response = new InteractionResponse(InteractionCallbackType.Modal, new(data));

        var res = await _interactions.CreateInteractionResponseAsync(_context.ID, _context.Token, response);

        return res;
    }
}