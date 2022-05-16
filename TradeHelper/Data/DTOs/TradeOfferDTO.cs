using Remora.Rest.Core;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.DTOs;

public record TradeOfferDTO(Guid ID, TradeStatus Status, Snowflake GuildID, Snowflake OwnerID, int OwnerReputation, Snowflake? ClaimerID, DateTime CreatedAt, DateTime? ClaimedAt, DateTime? CompletedAt, DateTime? ExpiresAt, string Offering, string Requesting);