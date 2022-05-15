using MediatR;
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
            var trade = await _db.Trades.FindAsync(request.ID);

            if (trade is null)
                return Result<TradeOfferDTO>.FromError(new NotFoundError($"There is no trade by the ID of {request.ID}."));
            
            
            trade.Status = request.Status;
            trade.ClaimerID = request.ClaimerID;

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