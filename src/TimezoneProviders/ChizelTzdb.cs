using System.Collections.ObjectModel;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Timezones
{
  public class ChizelTzdb : IDateTimeZoneProvider
  {
    private

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
}