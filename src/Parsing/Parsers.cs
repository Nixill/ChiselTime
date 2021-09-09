using System;
using System.Text.RegularExpressions;
using Nixill.Utils;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Parsing
{
  public static class Parsers
  {
    public static Regex RgxTime = new Regex(@"^(\d\d?)[:,. h時]*(\d\d)[:,. m分]*(?:(\d\d)s?)? ?(?:([ap])(?:m|.|.m.))?$");
    public static Regex RgxDate = new Regex(@"^(?:(\d\d(?:\d\d)?)[-\. /y年]*)?(\d?\d)(?:[-\. /m月]+(\d?\d)|(\d\d))[d日]?$");

    public static (LocalTime, bool) ParseTime(string time, ZonedDateTime now)
    {
      time = time.ToLower();
      if (!RgxTime.TryMatch(time, out Match match))
        throw new ArgumentException("The input is not a valid time: " + time);

      int hour = int.Parse(match.Groups[1].Value);
      int minute = int.Parse(match.Groups[2].Value);
      int second = 0;

      if (match.TryGroup(3, out string secondTxt)) second = int.Parse(secondTxt);

      if (match.TryGroup(4, out string ampm))
      {
        hour %= 12;
        if (ampm == "p")
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
      if (date != null)
      {
        date = date.ToLower();
        if (!RgxDate.TryMatch(date, out Match match))
          throw new ArgumentException("The input is not a valid date: " + date);

        int year = 0;
        int month = int.Parse(match.Groups[2].Value);
        int day = 0;

        if (match.TryGroup(3, out string dayStr)) day = int.Parse(dayStr);
        else day = int.Parse(match.Groups[4].Value);

        // Get the year using the following rules:
        // If the year is omitted, use the next occurrence of the given month and day.
        if (!match.TryGroup(1, out string yearStr))
        {
          LocalDate today = now.Date;
          LocalDate asParsed = new LocalDate(today.Year, month, day);

          if (asParsed < today) asParsed = asParsed.PlusYears(1);

          return asParsed;
        }

        // Otherwise, if it's a two digit year, resolve it to within 50 years. (Exactly 50 off, pick the future.)
        year = int.Parse(yearStr);

        if (yearStr.Length == 2)
        {
          LocalDate today = now.Date;
          int thisYear = today.Year;

          year += thisYear - (thisYear % 100);

          if (year < thisYear - 49) year += 100;
          if (year > thisYear + 50) year -= 100;
        }

        return new LocalDate(year, month, day);
      }
      else
      {
        // If no date was specified, make it today — or tomorrow if the date has already passed this year.
        LocalDate ret = now.LocalDateTime.Date;

        if (next) return ret.PlusDays(1);
        else return ret;
      }
    }
  }
}