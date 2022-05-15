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
using TradeHelper.Services;

namespace TradeHelper.Commands;

[Group("trade")]
public class TradeCommands : CommandGroup
{
    private readonly ITradeService _trades;
    private readonly InteractionContext _context;
    private readonly IDiscordRestUserAPI _users;
    private readonly IDiscordRestChannelAPI _chennels;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public TradeCommands(ITradeService trades, InteractionContext context, IDiscordRestUserAPI users, IDiscordRestChannelAPI chennels, IDiscordRestInteractionAPI interactions)
    {
        _trades = trades;
        _context = context;
        _users = users;
        _chennels = chennels;
        _interactions = interactions;
    }
    
    [Ephemeral]
    [Command("claim")]
    [Description("Claims a trade offer")]
    public async Task<IResult> Claim
    (
        [Option("claim_id")]
        [Description("The trade to claim.")]
        string rawClaimID
    )
    {
        if (!Guid.TryParse(rawClaimID, out var claimID))
            return await _interactions.EditOriginalInteractionResponseAsync(_context.ApplicationID, _context.Token, "That is not a valid claim ID.");
        
        
        var claimResult = await _trades.ClaimTradeOfferAsync(claimID, _context.User.ID);
        
        if (!claimResult.IsDefined(out var claim))
            return await _interactions.EditOriginalInteractionResponseAsync(_context.ApplicationID, _context.Token, claimResult.Error.Message);

        var channelResult = await _users.CreateDMAsync(claimResult.Entity.OwnerID);

        // If we can't DM them, don't worry about it. | TODO: Break this into a service?
        if (channelResult.IsDefined(out var channel))
        {
            await _chennels.CreateMessageAsync
            (
                channel.ID,
             $"Your trade offer (ID: `{claim.ID}`) has been claimed by <@{_context.User.ID}> (**{_context.User.Username}#{_context.User.Discriminator:0000}**).\n" +
                $"If the trade is not completed within a reasonable amount of time, you can revoke it by running `/trade cancel {claim.ID}`."
            );
        }
        
        return await _interactions.EditOriginalInteractionResponseAsync
        (
            _context.ApplicationID,
            _context.Token,
            "Successfully claimed the trade offer.\n" +
            "The owner of this trade will be notified that you have claimed it."    
        );

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