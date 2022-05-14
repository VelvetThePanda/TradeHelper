using MediatR;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.MediatR.Trades;

public static class GetTradeOffer
{
    public record Request(Guid TradeId) : IRequest<TradeOfferDTO?>;
    
    internal class Handler : IRequestHandler<Request, TradeOfferDTO?>
    {
        private readonly TradeContext _db;
        public Handler(TradeContext db) => _db = db;

        public async Task<TradeOfferDTO?> Handle(Request request, CancellationToken cancellationToken)
        {
            var trade = await _db.Trades.FindAsync(request.TradeId);
            
            if (trade is null)
                return null;

            return TradeOffer.ToDTO(trade);
        }
    }
}