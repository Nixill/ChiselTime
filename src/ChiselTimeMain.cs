using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Nixill.Discord.ChiselTime.Commands;
using NodaTime;

namespace Nixill.Discord.ChiselTime
{
  public class ChiselTimeMain
  {
    internal static DiscordClient Discord;
    internal static SlashCommandsExtension Commands;

    internal static ulong Owner;

    internal static IClock Clock;
    internal static IClock SysClock;

    static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

    public static async Task MainAsync()
    {
      // Let's get the bot set up
#if DEBUG
      string botToken = File.ReadAllLines("cfg/debug_token.cfg")[0];
#else
      string botToken = File.ReadAllLines("cfg/token.cfg")[0];
#endif

      // Start the clock
      // Non-debug mode uses the system clock only; debug mode clock can be changed with a command.
      SysClock = SystemClock.Instance;
      Clock = SysClock;

      Discord = new DiscordClient(new DiscordConfiguration()
      {
        Token = botToken,
        TokenType = TokenType.Bot
      });

      Commands = Discord.UseSlashCommands();

      await Discord.ConnectAsync();

#if DEBUG
      Commands.RegisterCommands<TimeCommand>(608847976554692611L);
#else
      Commands.RegisterCommands<TimeCommand>();
#endif

      await Task.Delay(-1);
    }
  }
}