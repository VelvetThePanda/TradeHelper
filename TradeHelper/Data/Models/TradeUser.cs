using Remora.Rest.Core;
using TradeHelper.Data.DTOs;

namespace TradeHelper.Data.Models;

public class TradeUser
{
    public Snowflake ID { get; set; }
    
    public int Reputation { get; set; }
    
    public DateTime LastRepTime { get; set; }

    public List<TradeOffer> TradeOffers { get; set; } = new();

    public List<TradeOffer> ClaimedTrades { get; set; } = new();
    
    public TradeUserDTO ToDTO() => new(ID, Reputation, LastRepTime, TradeOffers.Select(TradeOffer.ToDTO).ToArray(), ClaimedTrades.Select(TradeOffer.ToDTO).ToArray());
}