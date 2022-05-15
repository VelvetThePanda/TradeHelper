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
        var items = trades.Skip((page - 1) * 10).Take(10);

        var embedList = new List<IEmbed>();

        var sb = new StringBuilder();

        foreach (var item in items)
        {
            sb.Clear();

            sb
                .AppendLine($"<@{item.OwnerID}> is offering **{item.Offering}**")
                .AppendLine($"In exchange of **{item.Requesting}**")
                .AppendLine($"Created: <t:{((DateTimeOffset)item.CreatedAt).ToUnixTimeSeconds()}:R>")
                .Append("Status: ");

            
            if (item.Status is TradeStatus.InProgress)
            {
                sb.AppendLine($"In Progress | Claimed by <@{item.ClaimerID}> <t:{((DateTimeOffset)item.ClaimedAt).ToUnixTimeSeconds()}:R>");
            }
            else if (item.Status is TradeStatus.Completed)
            {
                sb.AppendLine($"Completed <t:{((DateTimeOffset)item.CompletedAt).ToUnixTimeSeconds()}:R> by <@{item.ClaimerID}> <t:{((DateTimeOffset)item.ClaimedAt).ToUnixTimeSeconds()}:R>");
            }
            else
            {
                sb.AppendLine("**Unclaimed**");
            }

            var embed = new Embed
            {
                Title = $"Trade ID: {item.ID}",
                Description = sb.ToString(),
                Colour = item.Status switch
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
}