using DiscordBotTemplate.Database;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Commands
{
    public class Basic : BaseCommandModule
    {
        [Command("store")]
        public async Task TestCommand(CommandContext ctx) 
        {
            var DBEngine = new DBEngine();

            var userInfo = new DUser
            {
                userName = ctx.User.Username,
                serverName = ctx.Guild.Name,
                serverID = ctx.Guild.Id
            };

            var isStored = await DBEngine.StoreUserAsync(userInfo);

            if (isStored == true)
            {
                await ctx.Channel.SendMessageAsync("Succesfully stored in Database");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Something went wrong");
            }
        }

        [Command("profile")]
        public async Task Profile(CommandContext ctx)
        {
            var DBEngine = new DBEngine();

            var userToRetrieve = await DBEngine.GetUserAsync(ctx.User.Username, ctx.Guild.Id);
            if (userToRetrieve.Item1 == true)
            {
                var profileEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.DarkBlue,
                    Title = $"{userToRetrieve.Item2.userName}'s Profile",
                    Description = $"Server Name: {userToRetrieve.Item2.serverName} \n" +
                                  $"Server ID: {userToRetrieve.Item2.serverID}"
                };

                await ctx.Channel.SendMessageAsync(embed: profileEmbed);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Something went wrong when getting your Profile");
            }
        }
    }
}
