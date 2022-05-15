namespace TradeHelper.Infrastructure;

/// <summary>
/// Represents a filter for viewing the list trade offers.
/// </summary>
public enum TradeViewType
{
    /// <summary>
    /// All offers.
    /// </summary>
    Global,
    
    /// <summary>
    /// Offers for the current user.
    /// </summary>
    Self,
    
    /// <summary>
    /// Offers by a specific user.
    /// </summary>
    User
}