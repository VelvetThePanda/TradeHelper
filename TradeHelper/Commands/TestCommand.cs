using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace TradeHelper.Commands;

[DiscordDefaultDMPermission(false)]
[DiscordDefaultMemberPermissions(DiscordPermission.ManageChannels)]
public class TestCommand : CommandGroup
{
    private readonly InteractionContext _context;
    private readonly IDiscordRestInteractionAPI _interactions;
    
    public TestCommand(InteractionContext context, IDiscordRestInteractionAPI interactions)
    {
        _context = context;
        _interactions = interactions;
    }
    
    [Ephemeral]
    [Command("test")]
    [Description("Test command")]
    public async Task<IResult> TestAsync() 
        => await _interactions.EditOriginalInteractionResponseAsync(_context.ApplicationID, _context.Token, "Congrats, you did the thing~");
}