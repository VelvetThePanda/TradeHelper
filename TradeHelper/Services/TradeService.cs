using MediatR;
using Remora.Rest.Core;
using Remora.Results;
using TradeHelper.Data.DTOs;

namespace TradeHelper.Services;

public class TradeService : ITradeService
{
    private readonly IMediator _mediator;
    
    public TradeService(IMediator mediator) => _mediator = mediator;

    public async Task<Result<TradeOfferDTO>> CreateTradeOfferAysync(Snowflake guildID, Snowflake creatorID, DateTime? expiresAt, string requesting, string offering) => default;
    public async Task<Result> ClaimTradeOfferAsync(Guid tradeOfferID, Snowflake userID) => default;
    public async Task<Result> UnclaimTradeOfferAsync(Guid tradeOfferID, Snowflake userID) => default;
    public async Task<Result> CancelTradeOfferAsync(Guid tradeOfferID, Snowflake userID) => default;
    public async Task<Result> CompleteTradeOfferAsync(Guid tradeOfferID, Snowflake userID) => default;
    public async Task<Result> DeleteTradeOfferAsync(Guid tradeOfferID, Snowflake userID) => default;
    public async Task<Result<IReadOnlyList<TradeOfferDTO>>> GetTradeOffersAsync(Snowflake guildID, Snowflake? userID) => default;
    public async Task<Result<TradeUserDTO>> GetTradeUserAsync(Snowflake userID) => default;
    public async Task<Result> RepUserAsync(Snowflake userID, bool positive) => default;
}