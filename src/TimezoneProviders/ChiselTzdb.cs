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
    private DictionaryGenerator<string, ChiselZone> Keywords = new(new Dictionary<string, ChiselZone>(), new DefaultGenerator<string, ChiselZone>());

    public static ChiselTzdb Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new ChiselTzdb();
        }
        return _Instance;
      }
    }

    public DateTimeZone this[string id] => throw new System.NotImplementedException();

    public string VersionId => throw new System.NotImplementedException();

    public ReadOnlyCollection<string> Ids => throw new System.NotImplementedException();

    public DateTimeZone GetSystemDefault()
    {
      throw new System.NotImplementedException();
    }

    public DateTimeZone GetZoneOrNull(string id)
    {
      throw new System.NotImplementedException();
    }
  }

  internal class ChiselZone
  {

  }
}