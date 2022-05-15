using MediatR;
using Remora.Rest.Core;
using Remora.Results;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.MediatR.Trades;
using TradeHelper.Data.MediatR.Users;
using TradeHelper.Data.Models;

namespace TradeHelper.Services;

public class TradeService : ITradeService
{
    private readonly IMediator _mediator;
    
    public TradeService(IMediator mediator) => _mediator = mediator;

    public async Task<Result<TradeOfferDTO>> CreateTradeOfferAysync(Snowflake guildID, Snowflake userID, DateTime? expiresAt, string requesting, string offering)
    {
        await _mediator.Send(new CreateUser.Request(userID));

        return await _mediator.Send(new CreateTradeOffer.Request(guildID, userID, expiresAt, requesting, offering));
    }

    public async Task<Result<TradeOfferDTO>> ClaimTradeOfferAsync(Guid tradeOfferID, Snowflake userID)
    {
        await _mediator.Send(new CreateUser.Request(userID));
        
        var trade = await _mediator.Send(new GetTradeOffer.Request(tradeOfferID));

        if (trade is null)
            return Result<TradeOfferDTO>.FromError(new NotFoundError("A trade with that ID doesn't exist."));

        if (trade.OwnerID == userID)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can't claim your own trade. Perhaps you meant to delist it?"));
        
        if (trade.Status is TradeStatus.InProgress)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade is currently in progress. If it becomes unclaimed, you can claim it."));
        
        if (trade.Status is TradeStatus.Completed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade has already been completed."));

        return await _mediator.Send(new UpdateTradeOffer.Request(tradeOfferID, userID, TradeStatus.InProgress));
    }

    public async Task<Result<TradeOfferDTO>> UnclaimTradeOfferAsync(Guid tradeOfferID, Snowflake userID)
    {
        await _mediator.Send(new CreateUser.Request(userID));
        
        var trade = await _mediator.Send(new GetTradeOffer.Request(tradeOfferID));

        if (trade is null)
            return Result<TradeOfferDTO>.FromError(new NotFoundError("A trade with that ID doesn't exist."));

        if (trade.OwnerID == userID)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can't un-claim your own trade. Perhaps you meant to delist it?"));
        
        if (trade.Status is TradeStatus.Unclaimed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade is currently wasn't claimed to begin with."));
        
        if (trade.ClaimerID != userID)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can only un-claim trades you claimed."));

        if (trade.Status is TradeStatus.Completed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade has already been completed."));

        return await _mediator.Send(new UpdateTradeOffer.Request(tradeOfferID, null, TradeStatus.Unclaimed));
    }

    public async Task<Result<TradeOfferDTO>> CancelTradeOfferAsync(Guid tradeOfferID, Snowflake userID)
    {
        await _mediator.Send(new CreateUser.Request(userID));
        
        var trade = await _mediator.Send(new GetTradeOffer.Request(tradeOfferID));

        if (trade is null)
            return Result<TradeOfferDTO>.FromError(new NotFoundError("A trade with that ID doesn't exist."));

        if (trade.OwnerID != userID)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can't cancel a trade you didn't create."));

        if (trade.Status is TradeStatus.Unclaimed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade is currently wasn't claimed to begin with."));
        
        if (trade.ClaimerID is null)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can only cancel a trade "));

        if (trade.Status is TradeStatus.Completed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("The trade has already been completed."));

        return await _mediator.Send(new UpdateTradeOffer.Request(tradeOfferID, null, TradeStatus.Unclaimed));
    }

    public async Task<Result<TradeOfferDTO>> CompleteTradeOfferAsync(Guid tradeOfferID, Snowflake userID)
    {
        await _mediator.Send(new CreateUser.Request(userID));
        
        var trade = await _mediator.Send(new GetTradeOffer.Request(tradeOfferID));

        if (trade is null)
            return Result<TradeOfferDTO>.FromError(new NotFoundError("A trade with that ID doesn't exist."));

        if (trade.OwnerID != userID)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can't complete a trade you didn't create."));

        if (trade.Status is TradeStatus.Unclaimed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade is currently wasn't claimed to begin with."));
        
        if (trade.ClaimerID is null)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade hasn't been claimed. Did you mean to delist it?"));

        if (trade.Status is TradeStatus.Completed)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("The trade has already been completed."));

        return await _mediator.Send(new UpdateTradeOffer.Request(tradeOfferID, null, TradeStatus.Completed));
    }

    public async Task<Result<TradeOfferDTO>> DeleteTradeOfferAsync(Guid tradeOfferID, Snowflake userID)
    {
        await _mediator.Send(new CreateUser.Request(userID));
        
        var trade = await _mediator.Send(new GetTradeOffer.Request(tradeOfferID));
        
        if (trade is null)
            return Result<TradeOfferDTO>.FromError(new NotFoundError("A trade with that ID doesn't exist."));
        
        if (trade.OwnerID != userID)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("You can't delist a trade you didn't create."));
        
        if (trade.Status is TradeStatus.Completed or TradeStatus.Delisted)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade has already been completed or delisted."));
        
        if (trade.Status is TradeStatus.InProgress)
            return Result<TradeOfferDTO>.FromError(new InvalidOperationError("This trade is currently in progress. You can't delist it."));
        
        return await _mediator.Send(new UpdateTradeOffer.Request(tradeOfferID, null, TradeStatus.Delisted));
    }

    public async Task<Result<IReadOnlyList<TradeOfferDTO>>> GetTradeOffersAsync(Snowflake guildID, Snowflake? userID)
    {
        var trades = await _mediator.Send(new GetTradeOffers.Request(guildID, userID));
        
        if (!trades.Any() && userID is not null)
            return Result<IReadOnlyList<TradeOfferDTO>>.FromError(new NotFoundError("That user doesn't have any active trades"));

        if (!trades.Any() && userID is null)
            return Result<IReadOnlyList<TradeOfferDTO>>.FromError(new NotFoundError("There aren't any active trades on the server. Come back later?"));

        return Result<IReadOnlyList<TradeOfferDTO>>.FromSuccess(trades);
    }

    public async Task<Result<TradeUserDTO>> GetTradeUserAsync(Snowflake userID) =>
        Result<TradeUserDTO>.FromSuccess(await _mediator.Send(new GetUser.Request(userID)));
    
    public async Task<Result> RepUserAsync(Snowflake userID, bool positive)
    {
        var user = await _mediator.Send(new GetUser.Request(userID));
        
        if (user is null)
            return Result.FromError(new NotFoundError("That user doesn't exist."));

        await _mediator.Send(new UpdateUser.Request(userID, positive));

        return Result.FromSuccess();
    }
}