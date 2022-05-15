using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Remora.Results;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.MediatR.Trades;

public static class UpdateTradeOffer
{
    public record Request(Guid ID, Snowflake? ClaimerID, TradeStatus Status) : IRequest<Result<TradeOfferDTO>>;

    
    internal class Handler : IRequestHandler<Request, Result<TradeOfferDTO>>
    {
        private readonly TradeContext _db;
        public Handler(TradeContext db) => _db = db;

        public async Task<Result<TradeOfferDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var trade = await _db.Trades
                .FirstOrDefaultAsync(t => t.ID == request.ID, cancellationToken);
            
            if (trade is null)
                return Result<TradeOfferDTO>.FromError(new NotFoundError($"There is no trade by the ID of {request.ID}."));
            
            trade.Status = request.Status;
            trade.ClaimerID = request.ClaimerID;
            
            trade.ClaimedAt = trade.Status switch
            {
                TradeStatus.Unclaimed => null,
                TradeStatus.InProgress when trade.ClaimedAt is not null => trade.ClaimedAt,
                TradeStatus.InProgress when trade.ClaimedAt is null => DateTime.UtcNow,
                _ => trade.ClaimedAt
            };
            
            if (trade.Status is TradeStatus.Completed)
                trade.CompletedAt ??= DateTime.UtcNow;

            try
            {
                await _db.SaveChangesAsync();
                return Result<TradeOfferDTO>.FromSuccess(TradeOffer.ToDTO(trade));
            }
            catch
            {
                return Result<TradeOfferDTO>.FromError(new InvalidOperationError("There was an error while trying to update the trade."));
            }
        }
    }
}