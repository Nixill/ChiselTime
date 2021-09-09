using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Nixill.Discord.Extensions
{
  public static class ContextExtensions
  {
    public static async Task ReplyAsync(this InteractionContext ctx, string message) =>
      await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
        new DiscordInteractionResponseBuilder().WithContent(message));

    public static async Task DeferAsync(this InteractionContext ctx) =>
      await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);

    public static async Task EditAsync(this InteractionContext ctx, string message) =>
      await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(message));

    public static async Task ReplyEphemeralAsync(this InteractionContext ctx, string message) =>
      await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
        new DiscordInteractionResponseBuilder().WithContent(message).AsEphemeral(true));
  }
}