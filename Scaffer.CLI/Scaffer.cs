using Scaffer.Core;
using Scaffer.Core.Helpers;

namespace Scaffer.CLI;

public class Scaffer
{
    private  void CheckFileExistence(string scaffPath)
    {
        if(!scaffPath.EndsWith(".scaff"))
        {
            scaffPath = scaffPath + ".scaff";
        }
            
        if (!File.Exists(scaffPath))
        {
            ConsoleHelper.WriteLineColor($"We could no find the specified fiel or path '{scaffPath}'", ConsoleColor.Red);
            return;
        }
    }

    private void CheckTemplateTag(ScaffFile file)
    {
        if (string.IsNullOrEmpty(file.Template))
        {
            ConsoleHelper.WriteLineColor("Your file is missing the <<temp> and <tempend>> marks", ConsoleColor.Red);
            return;
        }
    }
    
    
    
    

   
    public async Task Build(string scaffPath, Dictionary<string, string> parameters, string? scaffRoute, string? scaffOut)
    {
        
        try
        {
            
          
            if(!scaffPath.EndsWith(".scaff"))
            {
                scaffPath = scaffPath + ".scaff";
            }
            if (!File.Exists(scaffPath))
            {
                ConsoleHelper.WriteLineColor($"Error: We could no find the specefied fiel or path '{scaffPath}'", ConsoleColor.Red);
                return;
            }

            Console.WriteLine();
            
            ConsoleHelper.WriteLineColor($"* Building file from: {scaffPath}", ConsoleColor.Magenta);

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

            CheckTemplateTag(scaff);

            var finalOutputDir = scaffRoute ?? "./";

            if (string.IsNullOrEmpty(scaffRoute))
            {
                ConsoleHelper.WriteLineColor("Warning: You did not specify the output route (-r). The file will be created in the current directory.", ConsoleColor.DarkYellow);
            }

            string fullFileName;

            if (!string.IsNullOrEmpty(scaffOut))
            {
                
                fullFileName = scaffOut;
            }
            else
            {
                var randomName = $"output_{Guid.NewGuid().ToString("N").Substring(0, 6)}.txt";
                ConsoleHelper.WriteLineColor($"Warning: You did not specify the output name (-o). The file will be named: {randomName}", ConsoleColor.DarkYellow);
                Console.WriteLine();
                fullFileName = randomName;
            }

            Console.WriteLine($"\u001b[32mDirectory: \u001b[0m {finalOutputDir}");
            Console.WriteLine($"\u001b[32mFile Name: \u001b[0m {fullFileName}");

            if (parameters.Count > 0)
            {
                ConsoleHelper.WriteLineColor("Parameters: ", ConsoleColor.Green);
                foreach (var kvp in parameters)
                {
                    Console.WriteLine($"{kvp.Key} => {kvp.Value}");
                }
            }

            await ScaffGenerator.Generate(scaff, parameters, finalOutputDir, fullFileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}