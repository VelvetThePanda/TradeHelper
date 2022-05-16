using Remora.Rest.Core;

namespace TradeHelper.Data.DTOs;

public record TradeUserDTO(Snowflake ID, int Reputation, DateTime LastRepTime, IReadOnlyList<TradeOfferDTO> TradeOffers, IReadOnlyList<TradeOfferDTO> ClaimedTrades);