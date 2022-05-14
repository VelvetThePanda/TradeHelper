using Remora.Rest.Core;

namespace TradeHelper.Data.DTOs;

public record TradeOfferDTO(Guid ID, Snowflake GuildID, TradeUserDTO Owner, TradeUserDTO? Claimer, DateTime CreatedAt, DateTime? ClaimedAt, DateTime? ExpiresAt, string Offering, string Requesting);