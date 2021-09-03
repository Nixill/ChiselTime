using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Nixill.Discord.ChiselTime.Commands;

namespace Nixill.Discord.ShadowRoller
{
  public class ShadowRollerMain
  {
    internal static DiscordClient Discord;
    internal static SlashCommandsExtension Commands;

    internal static ulong Owner;

    static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

    public static async Task MainAsync()
    {
      // Let's get the bot set up
#if DEBUG
      string botToken = File.ReadAllLines("cfg/debug_token.cfg")[0];
#else
      string botToken = File.ReadAllLines("cfg/token.cfg")[0];
#endif

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