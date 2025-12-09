using Scaffer.Core;

namespace Scaffer;

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
            var scaffBuild = GetArgValue(args, "--scaff-build");
            var scaffRoute = GetArgValue(args, "--scaff-route");
            var scaffOut = GetArgValue(args, "--scaff-out");
            var scaffParams = GetAllParams(args, "--scaff-param");
            var scaffHelp = args.Any(a => a.ToLower() == "--scaff-help");
            var scaffList = args.Any(a => a.ToLower() == "--scaff-list");

            if (scaffHelp)
            {
                ShowHelp();
                return 0;
            }

            if (scaffList)
            {
                ListScaffFiles();
                return 0;
            }

          
            if (scaffBuild != null)
            {
                await BuildScaff(scaffBuild, scaffParams, scaffRoute, scaffOut);
                return 0;
            }

            ConsoleHelper.WriteLineColor("Invalid command", ConsoleColor.Red);
            Console.WriteLine("Use '--scaff-help' to see available commands");
            return 1;
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteLineColor($"Error {ex.InnerException?.Message ?? ex.Message}", ConsoleColor.Red);
            return 1;
        }
    }
    
    private static Dictionary<string, string> GetAllParams(string[] args, string argName)
    {
        var parameters = new Dictionary<string, string>();
        
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].ToLower() == argName.ToLower() && i + 1 < args.Length)
            {
                var paramValue = args[i + 1];
                var parts = paramValue.Split('=', 2);
                
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    parameters[key] = value;
                }
            }
        }
        
        return parameters;
    }
    private static string? GetArgValue(string[] args, string argName)
    {
        var index = Array.FindIndex(args, a => a.ToLower() == argName.ToLower());
        if (index >= 0 && index + 1 < args.Length)
        {
            return args[index + 1];
        }
        return null;
    }
    
     private static async Task BuildScaff(string scaffPath, Dictionary<string, string> parameters, string? scaffRoute, string? scaffOut)
    {
        try
        {
            if(!scaffPath.EndsWith(".scaff"))
            {
                scaffPath = scaffPath + ".scaff";
            }
            if (!File.Exists(scaffPath))
            {
                ConsoleHelper.WriteLineColor($"We could no fint the specefied fiel or path '{scaffPath}'", ConsoleColor.Red);
                return;
            }
            

            Console.WriteLine();
            ConsoleHelper.WriteLineColor($"Building file from: {scaffPath}", ConsoleColor.Blue);

            var content = await File.ReadAllTextAsync(scaffPath);
            var scaff = new ScaffFile();
            try
            { 
                scaff = ScaffParser.Parse(content);

            }
            catch(Exception ex)
            {
                ConsoleHelper.WriteLineColor(ex.InnerException?.Message ?? ex.Message, ConsoleColor.Red);
                return;

            }
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (string.IsNullOrEmpty(scaff.Template))
            {
                ConsoleHelper.WriteLineColor("Your file is missing the <<temp> and <tempend>> marks", ConsoleColor.Red);
                return;
            }

            var finalOutputDir = scaffRoute ?? scaff.Meta.GetValueOrDefault("Output", "./");

            if (string.IsNullOrEmpty(scaffRoute))
            {
                ConsoleHelper.WriteLineColor("You did not specified the output route. The created file will be added to the current directory or the one from the Output @Meta field", ConsoleColor.DarkYellow);
            }
            var finalFileName = scaffOut ?? scaff.Meta.GetValueOrDefault("Name", $"output{new Guid().ToString()}");
            
            if (string.IsNullOrEmpty(scaffOut))
            {
                ConsoleHelper.WriteLineColor($"You did not specified the name of the file. The created file will be named as output{new Guid().ToString()} or the one specified in the Name @Meta field", ConsoleColor.DarkYellow);
            }
            var extension = scaff.Meta.GetValueOrDefault("Extension", ".txt");
            
           
            
            if (!string.IsNullOrEmpty(scaffOut) && Path.HasExtension(scaffOut))
            {
                extension = "";
            }

            Console.WriteLine($"\u001b[32mDirectory: \u001b[0m {finalOutputDir}");
            Console.WriteLine($"\u001b[32mExtension: \u001b[0m {extension}");
            Console.WriteLine($"\u001b[32mFile Name: \u001b[0m {finalFileName}{extension}");

            
            if (parameters.Count > 0)
            {
                ConsoleHelper.WriteLineColor("Parameters: ", ConsoleColor.Green);
                foreach (var kvp in parameters)
                {
                    Console.WriteLine($"{kvp.Key} => {kvp.Value}");
                }
            }

            await ScaffGenerator.Generate(scaff, parameters, finalOutputDir, finalFileName + extension);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
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
                
                if (scaff.Meta.ContainsKey("Name"))
                    Console.WriteLine($"    Name: {scaff.Meta["Name"]}");
                if (scaff.Meta.ContainsKey("Extension"))
                    Console.WriteLine($"    Extension: {scaff.Meta["Extension"]}");
                if (scaff.Meta.ContainsKey("Output"))
                    Console.WriteLine($"    Output: {scaff.Meta["Output"]}");
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
        Console.WriteLine("Scaffer - Template Scaffolding CLI");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  scaffer [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --scaff-build <path>         Path to the .scaff file to build");
        Console.WriteLine("  --scaff-route <path>         Output directory for the generated file");
        Console.WriteLine("  --scaff-out <name>           Name of the output file");
        Console.WriteLine("  --scaff-param <key=value>    Template parameter (repeatable)");
        Console.WriteLine("  --scaff-init <name>          Creates a sample .scaff file");
        Console.WriteLine("  --scaff-list                 Lists all .scaff files");
        Console.WriteLine("  --scaff-help                 Shows this help message");
        Console.WriteLine();
       
        Console.WriteLine("Full example:");
        Console.WriteLine("  scaffer --scaff-build ./templates/page.scaff");
        Console.WriteLine("        --scaff-param title=Home");
        Console.WriteLine("        --scaff-param content=\"Hello World\" ");
        Console.WriteLine("        --scaff-route ./output");
        Console.WriteLine("        --scaff-out index.html");
    }

   
}

