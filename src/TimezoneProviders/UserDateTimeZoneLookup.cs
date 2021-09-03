using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Nixill.Discord.ChiselTime.Timezones
{
  public class UserDateTimeZoneLookup
  {
    private const string GetCmd =
      @"SELECT tzZone
        FROM userZones
        WHERE tzName = $name;";

    private const string SetCmd =
      @"INSERT OR REPLACE INTO userZones
        (tzName, tzZone)
        VALUES ($name, $zone);";

    private const string DelCmd =
      @"DELETE FROM userZones
        WHERE tzName = $name;";

    private static UserDateTimeZoneLookup _Instance;
    private SqliteConnection conn;

    public static async Task<UserDateTimeZoneLookup> GetInstance()
    {
      if (_Instance == null)
      {
        _Instance = new UserDateTimeZoneLookup();
        _Instance.conn = new SqliteConnection("Data Source=cfg/zones.db");
        await _Instance.conn.OpenAsync();
      }
      return _Instance;
    }
  }
}