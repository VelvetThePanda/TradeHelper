using Remora.Rest.Core;
using TradeHelper.Data.DTOs;

namespace TradeHelper.Data.Models;

public class TradeOffer
{
    /// <summary>
    /// The ID of this trade.
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// The status of the trade.
    /// </summary>
    public TradeStatus Status { get; set; }
    
    /// <summary>
    /// The guild the trade is created on.
    /// </summary>
    public Snowflake GuildID { get; set; }
    
    /// <summary>
    /// The ID of the owner of the trade.
    /// </summary>
    public Snowflake OwnerID { get; set; }
    
    /// <summary>
    /// The owner of the trade.
    /// </summary>
    public TradeUser Owner { get; set; }
    
    /// <summary>
    /// The ID of the claimer of the trade.
    /// </summary>
    public Snowflake? ClaimerID { get; set; }
    
    /// <summary>
    /// The claimer of the trade.
    /// </summary>
    public TradeUser? Claimer { get; set; }
    
    /// <summary>
    /// When the trade was claimed.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the trade was claimed.
    /// </summary>
    public DateTime? ClaimedAt { get; set; }
    
    /// <summary>
    /// When the trade was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// When the trade expires, if ever.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// What the owner is giving.
    /// </summary>
    public string Offering { get; set; }
    
    /// <summary>
    /// What the owner wants.
    /// </summary>
    public string Requesting { get; set; }
    
    public static TradeOfferDTO ToDTO(TradeOffer to) => new(to.ID, to.Status, to.GuildID, to.OwnerID, to.ClaimerID, to.CreatedAt, to.ClaimedAt, to.ExpiresAt, to.Offering, to.Requesting);
}