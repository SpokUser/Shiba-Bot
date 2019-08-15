﻿using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ShibaBot.Services;
using Discord.WebSocket;
using Discord.Commands;


namespace ShibaBot {
    class Program {
        static void Main() {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        async Task MainAsync() {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(new DiscordSocketClient());
            services.AddSingleton(new CommandService());
            services.AddSingleton<CommandHandler>();
            services.AddSingleton<StartupService>();
            services.AddSingleton<LoggingService>();

            ServiceProvider provider = services.BuildServiceProvider();
            provider.GetRequiredService<LoggingService>();
            provider.GetRequiredService<CommandHandler>();
            await provider.GetRequiredService<StartupService>().RunAsync();

            await Task.Delay(-1);
        }

    }
}
