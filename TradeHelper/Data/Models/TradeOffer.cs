using Remora.Rest.Core;

namespace TradeHelper.Data.Models;

public class TradeOffer
{
    /// <summary>
    /// The ID of this trade.
    /// </summary>
    public Guid Id { get; set; }
    
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
    public TradeUser Claimer { get; set; }
    
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
    /// What the trade will exchange for upon receving <see cref="TradeReceive"/>.
    /// </summary>
    public string TradeGiving { get; set; }
    
    /// <summary>
    /// What the trade desires in echange for <see cref="TradeGiving"/>.
    /// </summary>
    public string TradeReceive { get; set; }
    
}