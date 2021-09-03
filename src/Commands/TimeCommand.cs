using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace Nixill.Discord.ChiselTime.Commands
{
  [SlashCommandGroup("time", "Commands for using timestamp codes")]
  public class TimeCommand : SlashCommandModule
  {
    [SlashCommand("code", "Gets the code to put your timestamp in a message")]
    public async Task TimeCode(InteractionContext ctx,
      [Option("time", "The time to view")] string time,
      [Option("date", "The date to view; defaults to the next time the specified time occurs")] string date = null,
      [Option("zone", "The time zone to use; defaults to the user's or UTC if not set")] string timezone = null,
      [Option("dst", "Whether or not a time is within DST. ONLY NEEDED for times in the overlap when clocks are turned back. Ignored otherwise.")] bool dst = false)
    {

    }
  }
}