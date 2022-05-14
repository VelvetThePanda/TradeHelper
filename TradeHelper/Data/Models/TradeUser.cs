using Remora.Rest.Core;

namespace TradeHelper.Data.Models;

public class TradeUser
{
    public Snowflake ID { get; set; }
    
    public int Reputation { get; set; }
    
    public List<TradeOffer> TradeOffers { get; set; }
    
    public List<TradeOffer> ClaimedTrades { get; set; }
}