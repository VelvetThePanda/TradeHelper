using System.Drawing;
using System.Text;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;

namespace TradeHelper.Infrastructure;

public static class TradePaginationHelper
{
    public static void GetEmbedsAndComponents(IReadOnlyList<TradeOfferDTO> trades, int page, string type, Snowflake? userID, out IReadOnlyList<IEmbed> embeds, out IReadOnlyList<IMessageComponent> components)
    {
        var orderedTrades = OrderByReputation(trades);
        
        var listedTrade = orderedTrades.Skip((page - 1) * 10).Take(10);

        var sb = new StringBuilder();
        var embedList = new List<IEmbed>();
        
        foreach (var trade in listedTrade)
        {
            sb.Clear();

            sb
            .AppendLine($"<@{trade.OwnerID}> is offering **{trade.Offering}**")
            .AppendLine($"In exchange of **{trade.Requesting}**")
            .AppendLine($"Created: <t:{((DateTimeOffset)trade.CreatedAt).ToUnixTimeSeconds()}:R>")
            .Append("Status: ");
            
            if (trade.Status is TradeStatus.InProgress)
            {
                sb.AppendLine($"In Progress | Claimed by <@{trade.ClaimerID}> <t:{((DateTimeOffset)trade.ClaimedAt).ToUnixTimeSeconds()}:R>");
            }
            else if (trade.Status is TradeStatus.Completed)
            {
                sb.AppendLine($"Completed <t:{((DateTimeOffset)trade.CompletedAt).ToUnixTimeSeconds()}:R> by <@{trade.ClaimerID}> <t:{((DateTimeOffset)trade.ClaimedAt).ToUnixTimeSeconds()}:R>");
            }
            else
            {
                sb.AppendLine("**Unclaimed**");
            }

            var embed = new Embed
            {
                Title = $"Trade ID: {trade.ID}",
                Description = sb.ToString(),
                Colour = trade.Status switch
                {
                    TradeStatus.Unclaimed => Color.FromArgb(66, 242, 245),
                    TradeStatus.InProgress => Color.FromArgb(245, 221, 66),
                    TradeStatus.Completed => Color.FromArgb(66, 245, 93),
                    TradeStatus.Delisted => Color.FromArgb(194, 16, 16) // Should never happen, but just in case.
                }
            };

            embedList.Add(embed);
        }

        var buttons = new List<ButtonComponent>()
        {
            new(ButtonComponentStyle.Primary, "Back", CustomID: $"trade-list-{type}-page{page - 1}-{userID}"),
            new(ButtonComponentStyle.Primary, "Next", CustomID: $"trade-list-{type}-page{page + 1}-{userID}")
        };

        components = new IMessageComponent[1] { new ActionRowComponent(buttons) };

        if (page <= 1)
            buttons[0] = buttons[0] with { IsDisabled = true };

        if (page >= (int)(trades.Count / 10))
            buttons[1] = buttons[1] with { IsDisabled = true };
        
        embeds = embedList;
    }

    private static IEnumerable<TradeOfferDTO> OrderByReputation(IReadOnlyList<TradeOfferDTO> trades)
    {
        var tradeCounts = trades.ToDictionary(t => t.OwnerID, t => trades.Count(tc => tc.OwnerID == t.OwnerID));

        return trades.OrderByDescending(t =>
        {
            var ret = t.OwnerReputation / tradeCounts[t.OwnerID] + 0.15;

            Console.WriteLine($"Owner Rep: {t.OwnerReputation} / Trades: {tradeCounts[t.OwnerID]} + 0.15 = {ret}");
            
            return ret;
        });
    }
}