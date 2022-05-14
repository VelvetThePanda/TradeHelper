using Remora.Rest.Core;

namespace TradeHelper.Data.DTOs;

public record TradeUserDTO(Snowflake ID, int Reputation, IReadOnlyList<TradeOfferDTO> TradeOffers);