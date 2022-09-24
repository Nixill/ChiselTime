using System;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using Nixill.Discord.ChiselTime.Parsing;
using Nixill.Discord.ChiselTime.Timezones;
using Nixill.Discord.Extensions;
using NodaTime;
using NodaTime.Text;
using NodaTime.TimeZones;

namespace Nixill.Discord.ChiselTime.Commands
{
  [SlashCommandGroup("time", "Commands for using timestamp codes")]
  public class TimeCommand : ApplicationCommandModule
  {
    public static ZoneLocalMappingResolver DstResolver = Resolvers.CreateMappingResolver(Resolvers.ReturnEarlier, Resolvers.ReturnStartOfIntervalAfter);
    public static ZoneLocalMappingResolver NotDstResolver = Resolvers.CreateMappingResolver(Resolvers.ReturnLater, Resolvers.ReturnEndOfIntervalBefore);
    public static LocalTimePattern IsoTime = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");
    public static LocalDatePattern IsoDate = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd");

    [SlashCommand("code", "Gets the code to put your timestamp in a message")]
    public async Task TimeCode(InteractionContext ctx,
      [Option("time", "The time to view")] string timeStr,
      [Option("date", "The date to view; defaults to the next time the specified time occurs")] string dateStr = null,
      [Option("zone", "The time zone to use; defaults to the user's or UTC if not set")] string timezoneStr = null,
      [Option("dst", "Whether a time is during DST. ONLY NEEDED for times in an overlap or gap caused by a clock shift.")] bool? dst = null)
    {
      await ctx.ReplyEphemeralAsync("Just a moment please!");

      LocalTime time = default(LocalTime);
      LocalDate date = default(LocalDate);

      try
      {
        // First parse the time zone
        DateTimeZone zone = null;

        if (timezoneStr != null) zone = ChiselTzdb.Instance.GetZoneOrNull(timezoneStr);
        /*
        if (zone == null) zone = await UserDateTimeZoneLookup.GetInstance().GetZone(ctx.User.Id);
        // The second method returns UTC otherwise
        /*/
        if (zone == null) zone = DateTimeZone.Utc;
        // */

        // Get the current time in that zone
        ZonedDateTime now = ChiselTimeMain.Clock.GetCurrentInstant().InZone(zone);

        // Then the time
        (LocalTime Time, bool NextDay) timeOut = Parsers.ParseTime(timeStr, now);
        time = timeOut.Time;

        // Then the date
        date = Parsers.ParseDate(dateStr, timeOut.NextDay, now);

        // Combine them
        LocalDateTime ldt = date + time;

        ZonedDateTime zoned;

        // DST resolver
        if (dst.HasValue)
        {
          if (dst.Value) zoned = zone.ResolveLocal(ldt, DstResolver);
          else zoned = zone.ResolveLocal(ldt, NotDstResolver);
        }
        else
        {
          zoned = zone.AtStrictly(ldt);
        }

        // Now convert it to a Unix timestamp.
        long unix = zoned.ToInstant().ToUnixTimeSeconds();

        // And finally, send it to the user!
        await ctx.EditAsync($"`<t:{unix}>`");

        // Also send context to make sure it looks right.
        // Eventually there will be an option to disable this.

      }
      catch (AmbiguousTimeException)
      {
        await ctx.EditAsync($"{time} is an ambiguous time (occurs twice) on {date}. Use the `dst` parameter to pick a time:\n"
          + "• `True` selects the earlier of the times, before the clocks are changed.\n"
          + "• `False` selects the later of the times, after the clocks are changed.");
      }
      catch (SkippedTimeException)
      {
        await ctx.EditAsync($"{time} is a skipped time (does not occur) on {date}. Pick another time or use the `dst` parameter to select a time outside the gap:\n"
          + "• `True` selects the moment after the gap, once the clocks are changed.\n"
          + "• `False` selects the moment before the gap, before the clocks are changed.");
      }
      catch (ArgumentException)
      {
        if (dateStr != null) await ctx.EditAsync($"Either {timeStr} is not a valid time, or {dateStr} is not a valid date.");
        else await ctx.EditAsync($"{timeStr} is not a valid time.");
      }
      catch (Exception ex)
      {
        await ctx.EditAsync($"Something else went wrong: {ex}");
      }
    }
  }
}