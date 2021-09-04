using System.Text.RegularExpressions;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Parsing
{
  public static class Parsers
  {
    public static Regex RgxTime = new Regex(@"^(\d\d?)[:,. h]*(\d\d)[:,. m]*(?:(\d\d)s?)? ?(?:([ap])(?:m|.|.m.))?$");

    public static (LocalTime, bool) ParseTime(string time, ZonedDateTime now)
    {
      time = time.ToLower();

    }
  }
}