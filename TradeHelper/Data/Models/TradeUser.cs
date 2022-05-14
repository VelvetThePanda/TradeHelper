using Remora.Rest.Core;
using TradeHelper.Data.DTOs;

namespace TradeHelper.Data.Models;

public class TradeUser
{
    public Snowflake ID { get; set; }
    
    public int Reputation { get; set; }
    
    public List<TradeOffer> TradeOffers { get; set; }
    
    public List<TradeOffer> ClaimedTrades { get; set; }
    
    public TradeUserDTO ToDTO() => new(ID, Reputation, TradeOffers.Select(TradeOffer.ToDTO).ToArray(), ClaimedTrades.Select(TradeOffer.ToDTO).ToArray());
}