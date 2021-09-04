using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NodaTime;

namespace Nixill.Discord.ChiselTime.Timezones
{
  public class UserDateTimeZoneLookup
  {
    private const string GetCmd =
      @"SELECT tzZone
        FROM userZones
        WHERE tzUser = $user;";

    private const string SetCmd =
      @"INSERT OR REPLACE INTO userZones
        (tzUser, tzZone)
        VALUES ($user, $zone);";

    private const string DelCmd =
      @"DELETE FROM userZones
        WHERE tzUser = $user;";

    private static UserDateTimeZoneLookup Instance;
    private SqliteConnection Conn;
    private IDateTimeZoneProvider Tzdb;

    public static UserDateTimeZoneLookup GetInstance()
    {
      if (Instance == null)
      {
        throw new NullReferenceException("The instance has not been instantiated with StartInstance() yet.");
      }
      return Instance;
    }

    public static async Task StartInstance()
    {
      if (Instance != null)
      {
        throw new NotSupportedException("The instance has already been instantiated.");
      }

      Instance = new UserDateTimeZoneLookup();
      Instance.Conn = new SqliteConnection("Data Source=cfg/zones.db");

      Instance.Tzdb = DateTimeZoneProviders.Tzdb;

      await Instance.Conn.OpenAsync();
    }

    public async Task<DateTimeZone> GetZone(ulong uid)
    {
      var cmd = Conn.CreateCommand();
      cmd.CommandText = GetCmd;
      cmd.Parameters.AddWithValue("$user", uid);

      using (var reader = await cmd.ExecuteReaderAsync())
      {
        if (reader.Read())
        {
          return Tzdb[reader.GetString(0)];
        }
      }

      return DateTimeZone.Utc;
    }

    public async Task SetZone(ulong uid, DateTimeZone zone)
    {
      var cmd = Conn.CreateCommand();
      cmd.CommandText = SetCmd;
      cmd.Parameters.AddWithValue("$user", uid);
      cmd.Parameters.AddWithValue("$zone", zone.Id);

      await cmd.ExecuteNonQueryAsync();
    }

    public async Task DelZone(ulong uid)
    {
      var cmd = Conn.CreateCommand();
      cmd.CommandText = DelCmd;
      cmd.Parameters.AddWithValue("$user", uid);

      await cmd.ExecuteNonQueryAsync();
    }
  }
}