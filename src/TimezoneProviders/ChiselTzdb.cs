using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Nixill.Collections;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Timezones
{
  public class ChiselTzdb : IDateTimeZoneProvider
  {
    private static ChiselTzdb _Instance;
    private Dictionary<string, DateTimeZone> Keywords = new Dictionary<string, DateTimeZone>();
    IDateTimeZoneProvider Tzdb = DateTimeZoneProviders.Tzdb;

    public static ChiselTzdb Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new ChiselTzdb();
          Dictionary<string, DateTimeZone> copy = _Instance.Keywords;

          foreach (string id in _Instance.Tzdb.Ids)
          {
            foreach (string subid in Spliterate(id))
            {
              if (copy.ContainsKey(subid)) copy[subid] = null;
              else copy[subid] = _Instance.Tzdb[id];
            }
          }
        }
        return _Instance;
      }
    }

    public DateTimeZone this[string id] => Keywords[id.ToLower().Replace('-', ' ').Replace('_', ' ')];

    public string VersionId => Tzdb.VersionId;

    public ReadOnlyCollection<string> Ids => Tzdb.Ids;

    public DateTimeZone GetSystemDefault() => Tzdb.GetSystemDefault();

    public DateTimeZone GetZoneOrNull(string id)
    {
      id = id.ToLower().Replace('-', ' ').Replace('_', ' ');
      if (Keywords.ContainsKey(id)) return Keywords[id];
      else return null;
    }

    private static IEnumerable<string> Spliterate(string id)
    {
      int index = 1;
      id = id.ToLower().Replace('-', ' ').Replace('_', ' ');

      while (index > 0)
      {
        yield return id;
        index = id.IndexOf('/') + 1;
        id = id.Substring(index);
      }
    }
  }
}