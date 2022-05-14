namespace TradeHelper.Data.Models;

/// <summary>
/// Represents the status of a trade.
/// </summary>
public enum TradeStatus
{
    /// <summary>
    /// The trade is unclaimed and can be picked up by anyone.
    /// </summary>
    Unclaimed,
    
    /// <summary>
    /// The trade has been claimed, but has not been completed.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// The trade has been completed.
    /// </summary>
    Completed,
    
    /// <summary>
    /// The trade was delisted.
    /// </summary>
    Delisted
}