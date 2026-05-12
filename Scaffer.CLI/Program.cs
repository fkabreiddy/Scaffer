using Scaffer.Core;
using Scaffer.Core.Helpers;

namespace Scaffer.CLI;

internal class Program
{
    
    public static async Task<int> Main(string[] args)
    {
        
        
        if (args.Length == 0)
        {
            ShowHelp();
            return 0;
        }

        try
        {
            var build =  ConsoleHelper.GetArgValue(args, "-b", "--build");
            var route =  ConsoleHelper.GetArgValue(args, "-r", "--route");
            var output = ConsoleHelper.GetArgValue(args, "-o", "--out", "--output");
            var parameters = ConsoleHelper.GetParams(args, "-p");

            var help = args.Any(a =>
                a.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                a.Equals("--help", StringComparison.OrdinalIgnoreCase));

            var list = args.Any(a =>
                a.Equals("-l", StringComparison.OrdinalIgnoreCase) ||
                a.Equals("--list", StringComparison.OrdinalIgnoreCase));

            if (help)
            {
                ShowHelp();
                return 0;
            }

            if (list)
            {
                ListScaffFiles();
                return 0;
            }

          
            if (build != null)
            {
                var scaffer = new Scaffer();
                await scaffer.Build(build, parameters, route, output);
                return 0;
            }

            ConsoleHelper.WriteLineColor("Invalid command", ConsoleColor.Red);
            Console.WriteLine("Use '-h' to see available commands");
            return 1;
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteLineColor($"Error {ex.InnerException?.Message ?? ex.Message}", ConsoleColor.Red);
            return 1;
        }
    }
    
    private static void ListScaffFiles()
    {
        var scaffFiles = Directory.GetFiles(".", "*.scaff");
        
        if (scaffFiles.Length == 0)
        {
            ConsoleHelper.WriteLineColor("No .scaff files found on the current directory", ConsoleColor.Red);
            return;
        }

        ConsoleHelper.WriteLineColor($"{scaffFiles.Length} .scaff files found:", ConsoleColor.Green);
        Console.WriteLine();

        foreach (var file in scaffFiles)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine($"  • {name}");
            
            try
            {
                var content = File.ReadAllText(file);
                var scaff = ScaffParser.Parse(content);
           
            }
            catch(Exception ex)
            {
                ConsoleHelper.WriteLineColor(ex.InnerException?.Message ?? ex.Message, ConsoleColor.Red);
            }
            
            Console.WriteLine();
        }
    }
    
    private static void ShowHelp()
    {
        Console.WriteLine();

        Console.WriteLine("Scaffer - Universal Scaffolding Generator CLI");
        Console.WriteLine();

        Console.WriteLine("Usage:");
        Console.WriteLine("  scaffer [command] [options]");
        Console.WriteLine();

        Console.WriteLine("Commands:");
        Console.WriteLine("  -b, --build <template>       Builds a .scaff template");
        Console.WriteLine("  -l, --list                   Lists all available .scaff files");
        Console.WriteLine("  -h, --help                   Shows this help message");
        Console.WriteLine();

        Console.WriteLine("Options:");
        Console.WriteLine("  -r, --route <path>           Output directory");
        Console.WriteLine("  -o, --out <name>             Output file name");
        Console.WriteLine("  -p, --param <key=value>      Template parameter (repeatable)");
        Console.WriteLine();

        Console.WriteLine("Examples:");
        Console.WriteLine();

        Console.WriteLine("  scaffer -b feature");
        Console.WriteLine();

        Console.WriteLine("  scaffer -b feature -p name=User");
        Console.WriteLine();

        Console.WriteLine("  scaffer -b page");
        Console.WriteLine("          -p title=Home ");
        Console.WriteLine("          -p content=\"Hello World\" ");
        Console.WriteLine("          -r ./pages");
        Console.WriteLine("          -o index.html");
        Console.WriteLine();

        Console.WriteLine("Long syntax:");
        Console.WriteLine();

        Console.WriteLine("  scaffer --build page");
        Console.WriteLine("          --param title=Home");
        Console.WriteLine("          --route ./output");
        Console.WriteLine("          --out index.html");
        Console.WriteLine();
    }

    

   
}

