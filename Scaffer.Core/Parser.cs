using System.Text.RegularExpressions;
using Scaffer.Core.Helpers;

namespace Scaffer.Core;

public static class ScaffParser
{
    extension(string content)
    {
        public ScaffFile Parse()
        {
            var result = new ScaffFile();

            ParseParams(content, result);
            ParseTemplate(content, result);

            return result;
        }
    }
    
    
    private static void ParseParams(string content, ScaffFile result)
    {
        var match = Regex.Match(content, @"@Params\s*\(\s*([\s\S]*?)\s*\)", RegexOptions.Multiline);
        if (!match.Success)
        {
            ConsoleHelper.WriteLineColor("No @Params directive was found. Proceeding without default values.", ConsoleColor.Yellow);
            return;
        }

        Console.WriteLine();
        ConsoleHelper.WriteLineColor("* Obtaining @params...", ConsoleColor.Magenta);
        Console.WriteLine();

        var block = match.Groups[1].Value;
        var lineRegex = new Regex(@"^\s*([A-Za-z0-9_]+)\?\?(.*)\s*$", RegexOptions.Multiline);

        foreach (Match m in lineRegex.Matches(block))
        {
            var key = m.Groups[1].Value.Trim();
            var fallback = m.Groups[2].Value.Trim();

            if (result.Params.ContainsKey(key))
            {
                throw new ArgumentException($"Repeated parameter: '{key}'. Parameter names must be unique.");
            }
            
            result.Params[key] = fallback;
            ConsoleHelper.WriteLineColor($"  -p: {key} = {(string.IsNullOrEmpty(fallback) ? "(no default value)" : fallback)}", string.IsNullOrEmpty(fallback) ? ConsoleColor.Yellow : ConsoleColor.Green);
        }
        Console.WriteLine();
    }

    private static void ParseTemplate(string content, ScaffFile result)
    {
        var match = Regex.Match(content, @"<<temp\s*([\s\S]*?)\s*tempend>>", RegexOptions.Multiline);
        if (match.Success)
        {
            result.Template = match.Groups[1].Value.Trim();
        }
    }
  
}