using Remora.Rest.Core;
using Remora.Results;
using TradeHelper.Data.DTOs;

namespace TradeHelper.Services;

/// <summary>
/// Represents an interface for managing trade offers.
/// </summary>
public interface ITradeService
{
    /// <summary>
    /// Creates a new trade offer.
    /// </summary>
    /// <param name="guildID">The ID of the guild the trade is being created on.</param>
    /// <param name="userID">The ID of the creator of the trade.</param>
    /// <param name="expiresAt">When the trade expires, if ever.</param>
    /// <param name="requesting">What the trade requests.</param>
    /// <param name="offering">The offer of the trade, in return for <see cref="requesting"/>.</param>
    /// <returns>A result with the created trade, or an error.</returns>
    Task<Result<TradeOfferDTO>> CreateTradeOfferAysync(Snowflake guildID, Snowflake userID, DateTime? expiresAt, string requesting, string offering);
    
    /// <summary>
    /// Attempts to claim a trade offer.
    /// </summary>
    /// <param name="tradeOfferID">The ID of the trade to claim.</param>
    /// <param name="userID">The ID of the user that claimed the trade.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<TradeOfferDTO>> ClaimTradeOfferAsync(Guid tradeOfferID, Snowflake userID);

    /// <summary>
    /// Un-claims a trade offer.
    /// </summary>
    /// <param name="tradeOfferID">The ID of the trade to unclaim.</param>
    /// <param name="userID">The ID of the user that's unclaiming the trade.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<TradeOfferDTO>> UnclaimTradeOfferAsync(Guid tradeOfferID, Snowflake userID);
    
    /// <summary>
    /// Cancels a trade offer.
    /// </summary>
    /// <param name="tradeOfferID">The ID of the trade that's being cancelled.</param>
    /// <param name="userID">The ID of the user that cancelled the trade.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<TradeOfferDTO>> CancelTradeOfferAsync(Guid tradeOfferID, Snowflake userID);
    
    /// <summary>
    /// Completes a trade offer.
    /// </summary>
    /// <param name="tradeOfferID">The ID of the offer that's being completed.</param>
    /// <param name="userID">The ID of the user attempting to complete the trade.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<TradeOfferDTO>> CompleteTradeOfferAsync(Guid tradeOfferID, Snowflake userID);
    
    /// <summary>
    /// Unlists a trade offer.
    /// </summary>
    /// <param name="tradeOfferID">The ID of the trade that's being unlisted.</param>
    /// <param name="userID">The ID of the user attempting to unlist the trade.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<TradeOfferDTO>> DeleteTradeOfferAsync(Guid tradeOfferID, Snowflake userID);
    
    /// <summary>
    /// Gets all the available trade offers for a guild.
    /// </summary>
    /// <param name="guildID">The ID of the guild to query trades from.</param>
    /// <param name="userID">The ID of the user filter trades by.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<IReadOnlyList<TradeOfferDTO>>> GetTradeOffersAsync(Snowflake guildID, Snowflake? userID);
    
    /// <summary>
    /// Gets a trade user.
    /// </summary>
    /// <param name="userID">The ID of the user to get.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result<TradeUserDTO>> GetTradeUserAsync(Snowflake userID);

    /// <summary>
    /// Reps a user.
    /// </summary>
    /// <param name="userID">The ID of the user to rep.</param>
    /// <param name="positive">Whether the rep is positive.</param>
    /// <returns>A result that may or not have succeeded.</returns>
    Task<Result> RepUserAsync(Snowflake userID, bool positive);
}