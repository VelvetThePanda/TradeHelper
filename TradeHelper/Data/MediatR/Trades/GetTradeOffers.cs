using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.MediatR.Trades;

public static class GetTradeOffers
{
    public record Request(Snowflake GuildID, Snowflake? UserID = null) : IRequest<IReadOnlyList<TradeOfferDTO>>;
    
    internal class Handler : IRequestHandler<Request, IReadOnlyList<TradeOfferDTO>>
    {
        private readonly TradeContext _db;
        public Handler(TradeContext db) => _db = db;

        public async Task<IReadOnlyList<TradeOfferDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var tradeQuery = _db.Trades.Where(t => t.GuildID == request.GuildID);

            if (request.UserID is Snowflake ID) 
                tradeQuery = tradeQuery.Where(t => t.OwnerID == ID);

            var trades = await tradeQuery.ToListAsync();
            
            return trades.Where(t => t.Status is not (TradeStatus.Completed or TradeStatus.Delisted)).Select(TradeOffer.ToDTO).ToArray();
        }
    }
}