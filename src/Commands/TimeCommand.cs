using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using Nixill.Discord.ChiselTime.Parsing;
using Nixill.Discord.ChiselTime.Timezones;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Commands
{
  [SlashCommandGroup("time", "Commands for using timestamp codes")]
  public class TimeCommand : SlashCommandModule
  {
    [SlashCommand("code", "Gets the code to put your timestamp in a message")]
    public async Task TimeCode(InteractionContext ctx,
      [Option("time", "The time to view")] string timeStr,
      [Option("date", "The date to view; defaults to the next time the specified time occurs")] string dateStr = null,
      [Option("zone", "The time zone to use; defaults to the user's or UTC if not set")] string timezoneStr = null,
      [Option("dst", "Whether or not a time is during DST. ONLY NEEDED for times in the overlap when clocks are turned back. Ignored otherwise.")] bool? dst = null)
    {
      // First parse the time zone
      DateTimeZone zone = null;

      if (timezoneStr != null) zone = ChiselTzdb.Instance.GetZoneOrNull(timezoneStr);
      if (zone == null) zone = await UserDateTimeZoneLookup.GetInstance().GetZone(ctx.User.Id);
      // The second method returns UTC otherwise

      // Get the current time in that zone
      ZonedDateTime now = ChiselTimeMain.Clock.GetCurrentInstant().InZone(zone);

      // Then the time
      (LocalTime Time, bool NextDay) timeOut = Parsers.ParseTime(timeStr, now);
      LocalTime time = timeOut.Time;

      // Then the date
      var dateOut = Parsers.ParseDate(dateStr, timeOut.NextDay, now);
    }
  }
}