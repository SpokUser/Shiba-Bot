﻿using Discord;
using ShibaBot.Models;
using Discord.Commands;
using ShibaBot.Extensions;
using ShibaBot.Singletons;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShibaBot.Modules {
    public class HelpModule : ModuleBase<CommandContext> {
        [Command("help"), Alias("ajuda", "commands", "comandos")]
        public async Task HelpAsync(string commandName = null) {
            LocalesModel lang = await Language.GetLanguageAsync(Context);
            EmbedBuilder builder = new EmbedBuilder { Color = new Color(Utils.embedColor) };
            if (commandName is null) {
                builder.Description = lang.Modules.Help.Help.Replace("$prefix", await new UtilitiesExtension().GetPrefixAsync(Context));
                foreach (LocalesModel.ModulesModel.HelpModel.ModuleModel helpModule in lang.Modules.Help.Modules) {
                    List<string> commands = new List<string>();
                    foreach (string command in helpModule.Commands) {
                        commands.Add($"`{command}`");
                    }
                    builder.AddField(helpModule.Name, string.Join(", ", commands));
                }
            }
            else {
                bool hasCommandUse = new CommandUseExtension().EmbedCommandUse(ref builder, lang, commandName, await new UtilitiesExtension().GetPrefixAsync(Context));
                builder.Title = lang.Modules.Help.CommandHelp;
                
                if (!hasCommandUse) {
                    new CommandUseExtension().EmbedCommandUse(ref builder, lang, "help", await new UtilitiesExtension().GetPrefixAsync(Context));
                }
            }
            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
