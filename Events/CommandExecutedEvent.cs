﻿using System.Threading.Tasks;
using Discord.Commands;
using ShibaBot.Singletons;
using ShibaBot.Models;
using ShibaBot.Extensions;
using ShibaBot.Data.MySQL.DAO;
using Discord;

namespace ShibaBot.Events {
    public class CommandExecutedEvent {

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result) {
            if (!command.IsSpecified) return;
            
            if (!result.IsSuccess) {
                EmbedBuilder builder = new EmbedBuilder() { Color = new Color(Utils.embedColor) };

                LocalesModel locales = await Language.GetLanguageAsync((CommandContext)context);

                switch (result.Error) {
                    case CommandError.UnmetPrecondition:
                        switch (result.ErrorReason) {
                            case "GuildOnly":
                                builder.Title = locales.Errors.UnmetCondition.GuildOnly;
                                await context.Channel.SendMessageAsync(embed: builder.Build());
                                break;
                            case "UserManageGuild":
                                builder.Title = locales.Errors.UnmetCondition.UserManageGuild;
                                await context.Channel.SendMessageAsync(embed: builder.Build());
                                break;
                        }
                        break;
                    case CommandError.BadArgCount:
                        builder.Title = locales.Errors.BadArgCount;
                        bool hasCommandUse = new CommandUseExtension().EmbedCommandUse(ref builder, locales, command.Value.Name, await new UtilitiesExtension().GetPrefixAsync(context as CommandContext));
                        if (hasCommandUse) {
                            await context.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        break;
                    case CommandError.ObjectNotFound:
                        builder.Title = locales.Errors.ObjectNotFound;
                        bool hasCommandUse2 = new CommandUseExtension().EmbedCommandUse(ref builder, locales, command.Value.Name, await new UtilitiesExtension().GetPrefixAsync(context as CommandContext));
                        if (hasCommandUse2) {
                            await context.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        break;
                }
            }
        }
    }
}
