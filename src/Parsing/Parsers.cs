using System;
using System.Text.RegularExpressions;
using Nixill.Utils;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Parsing
{
  public static class Parsers
  {
    public static Regex RgxTime = new Regex(@"^(\d\d?)[:,. h]*(\d\d)[:,. m]*(?:(\d\d)s?)? ?(?:([ap])(?:m|.|.m.))?$");

    public static (LocalTime, bool) ParseTime(string time, ZonedDateTime now)
    {
      time = time.ToLower();
      if (!RgxTime.TryMatch(time, out Match match))
        throw new ArgumentException("The input is not a valid time: " + time);

      int hour = int.Parse(match.Groups[1].Value);
      int minute = int.Parse(match.Groups[2].Value);
      int second = 0;
      if (match.Groups[3].Success)
      {
        second = int.Parse(match.Groups[3].Value);
      }

      if (match.Groups[4].Success)
      {
        hour %= 12;
        if (match.Groups[4].Value == "p")
        {
          hour += 12;
        }
      }

      LocalTime input = new LocalTime(hour, minute, second);
      LocalTime comp = now.LocalDateTime.TimeOfDay;
      comp = comp.PlusSeconds(-comp.Second);

      if (input < comp) return (input, true);
      else return (input, false);
    }

    public static LocalDate ParseDate(string date, bool next, ZonedDateTime now)
    {

    }
  }
}