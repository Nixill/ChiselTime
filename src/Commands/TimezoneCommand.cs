using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using Nixill.Discord.Extensions;

namespace Nixill.Discord.ChiselTime.Commands
{
  [SlashCommandGroup("timezone", "Commands relating to time zones.")]
  public class TimezoneCommand : SlashCommandModule
  {
    [SlashCommand("set", "Set your time zone.")]
    public async Task SetTimeZone(InteractionContext ctx,
      [Option("zone", "The IANA Time Zone ID to use.")] string zone
    )
    {
      await ctx.ReplyEphemeralAsync("Just a moment please!");

      try
      {
        // Make sure time zone is valid
      }
    }
  }
}