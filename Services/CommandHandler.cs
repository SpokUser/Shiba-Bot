﻿using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using ShibaBot.Singletons;
using ShibaBot.Models;
using System.Threading;
using System.Text.RegularExpressions;
using Discord;
using System;

namespace ShibaBot.Services {
    public class CommandHandler {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider provider) {
            _client = client;
            _commands = commands;
            _provider = provider;

            _client.MessageReceived += MessageReceivedAsync;
        }

        private Task MessageReceivedAsync(SocketMessage socketMessage) {
            new Thread(async () => {
                SocketUserMessage message = (SocketUserMessage)socketMessage;

                if (message == null ||
                    message.Author.Id == _client.CurrentUser.Id) return;

                CommandContext context = new CommandContext(_client, message);

                int argPos = 0;

            if (message.HasStringPrefix("sh!", ref argPos, StringComparison.OrdinalIgnoreCase) ||
                message.HasStringPrefix("shiba ", ref argPos, StringComparison.OrdinalIgnoreCase) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) {
                IResult result = await _commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess) {
                    EmbedBuilder builder = new EmbedBuilder() { Color = Utils.embedColor };

                    LocalesModel locales = await Language.GetLanguageAsync(context);

                    switch (result.Error) {
                        case CommandError.UnmetPrecondition:
                            switch (result.ErrorReason) {
                                case "GuildOnly":
                                    builder.Title = locales.Errors.UnmetCondition.GuildOnly;
                                    await context.Channel.SendMessageAsync(embed: builder.Build());
                                    break;
                                case "UserManageGuild":
                                    await context.Channel.SendMessageAsync(embed: builder.Build());
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CommandError.BadArgCount:
                            builder.Title = locales.Errors.BadArgCount;
                            await context.Channel.SendMessageAsync(embed: builder.Build());
                            break;
                        case CommandError.ObjectNotFound:
                            builder.Title = locales.Errors.ObjectNotFound;
                            await context.Channel.SendMessageAsync(embed: builder.Build());
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (Regex.Match(message.Content, $"<@!?{_client.CurrentUser.Id}").Success) { 
                    EmbedBuilder builder = new EmbedBuilder() { Color = Utils.embedColor };
                    builder.Title = (await Language.GetLanguageAsync(context)).Mention;

                    await context.Channel.SendMessageAsync(embed: builder.Build());
                }
            }).Start();
            return Task.CompletedTask;
        }
    }
}
