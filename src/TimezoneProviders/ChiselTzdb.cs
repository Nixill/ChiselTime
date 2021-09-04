using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Timezones
{
  public class ChiselTzdb : IDateTimeZoneProvider
  {
    private Dictionary<string, DateTimeZone> Zones = new Dictionary<string, DateTimeZone>();

    public static readonly ChiselTzdb Instance = new ChiselTzdb();

    private ChiselTzdb()
    {
      IDateTimeZoneProvider tzdb = DateTimeZoneProviders.Tzdb;
      foreach (string id in tzdb.Ids)
      {
        foreach (string subId in Spliterate(id.ToLower()))
        {
          if (Zones.ContainsKey(subId)) Zones[subId] = null;
          else Zones[subId] = tzdb[id];
        }
      }
    }

    private IEnumerable<string> Spliterate(string id)
    {
      id.Replace('_', ' ').Replace('-', ' ');
      int index = 1;

      while (index > 0)
      {
        yield return id;
        index = id.IndexOf('/') + 1;
        id = id.Substring(index);
      }
    }

    public DateTimeZone this[string id] => Zones[id];

    public string VersionId => DateTimeZoneProviders.Tzdb.VersionId;

    public ReadOnlyCollection<string> Ids => new ReadOnlyCollection<string>(Zones.Where(x => x.Key != null).Select(x => x.Key).ToList());

    public DateTimeZone GetSystemDefault()
    {
      return DateTimeZoneProviders.Tzdb.GetSystemDefault();
    }

    public DateTimeZone GetZoneOrNull(string id)
    {
      return Zones.GetValueOrDefault(id);
    }
  }
}