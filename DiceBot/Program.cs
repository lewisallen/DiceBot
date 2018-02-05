using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace DiceBot {
    // invite link: https://discordapp.com/oauth2/authorize?client_id=409916169445834772&scope=bot
    class Program {
        // create the DiscordClient that will be the bot itself
        private static DiscordClient _discordClient;
        
        // create the variable to reference the commands module
        private static CommandsNextModule _commands;
        
        // call for the async method sot that the bot runs indefinitely
        public static void Main(string[] args) {
            // read from a config file for token and prefix
            try {
                ConfigReader.ReadConfig();
            } catch (ArgumentException e) {
                Console.WriteLine("Illegal config option. Ignoring.");
            }
            
            
            // startup the async method for continuous running
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // async method to handle bot running
        private static async Task MainAsync(string[] args) {
            // create the acual DiscordClient object.
            _discordClient = new DiscordClient(new DiscordConfiguration {
                // enable internal logging, no need for log4net anymore
                UseInternalLogHandler = true,
            #if DEBUG
                LogLevel = LogLevel.Debug,
            #else
                LogLevel = LogLevel.Info,
            #endif
                Token = ConfigReader.TOKEN,
                TokenType = TokenType.Bot
            });

            // enable the commands module
            _commands = _discordClient.UseCommandsNext(new CommandsNextConfiguration {
                StringPrefix = ConfigReader.PREFIX
            });
            
            // 
            _commands.RegisterCommands<DiceCommands>();
            
            // actually connect to the discord servers
            await _discordClient.ConnectAsync();
            
            // await for a delay of -1ms so that this mehtod is never terminated
            await Task.Delay(-1);
        }
    }
}