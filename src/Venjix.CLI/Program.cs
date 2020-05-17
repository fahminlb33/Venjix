using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Venjix.CLI
{
    class Program
    {
        static int Main(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand();
            rootCommand.Description = "My sample app";
            rootCommand.Add(new Option<int>(
                    "--int-option",
                    getDefaultValue: () => 42,
                    description: "An option whose argument is parsed as an int"));
            rootCommand.Add(new Option<bool>(
            "--bool-option",
            "An option whose argument is parsed as a bool"));
            var command = new Command("serialize");
            command.AddAlias("serialise");
            command.Description = "Serialize data";
            command.Handler = CommandHandler.Create(() => Console.WriteLine("Hello World!"));
            rootCommand.Add(command);

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<int, bool>((intOption, boolOption) =>
             {
                 Console.WriteLine($"The value for --int-option is: {intOption}");
                 Console.WriteLine($"The value for --bool-option is: {boolOption}");
             });

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
