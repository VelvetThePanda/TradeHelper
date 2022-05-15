using MediatR;
using Remora.Rest.Core;
using Remora.Results;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.MediatR.Trades;

public static class CreateTradeOffer
{
    public record Request(Snowflake GuildID, Snowflake OwnerID, DateTime? ExpiresAt, string Requesting, string Offering) : IRequest<Result<TradeOfferDTO>>;
    
    internal class Handler : IRequestHandler<Request, Result<TradeOfferDTO>>
    {
        private readonly TradeContext _db;
        
        public Handler(TradeContext db) => _db = db;

        public async Task<Result<TradeOfferDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var trade = new TradeOffer
            {
                GuildID = request.GuildID,
                OwnerID = request.OwnerID,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = request.ExpiresAt,
                Requesting = request.Requesting,
                Offering = request.Offering
            };

            _db.Trades.Add(trade);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
                return Result<TradeOfferDTO>.FromSuccess(TradeOffer.ToDTO(trade));
            }
            catch
            {
                return Result<TradeOfferDTO>.FromError(new InvalidOperationError("Failed to create trade offer."));
            }
        }
    }
}