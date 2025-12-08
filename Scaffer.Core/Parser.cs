using System.Text.RegularExpressions;

namespace Scaffer.Core;


public class ScaffFile
{
    public Dictionary<string, string> Meta { get; set; } = new();
    public string Template { get; set; } = "";

    public Dictionary<string, string> Params { get; set; } = new();
}
public static class ScaffParser
{
    public static ScaffFile Parse(string content)
    {
        var result = new ScaffFile();

        // Parse Meta
        var metaMatch = Regex.Match(content, @"@Meta\s*\(([\s\S]*?)\)", RegexOptions.Multiline);
        if (metaMatch.Success)
        {
            var lines = metaMatch.Groups[1].Value
                .Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Contains(":"))
                .ToList();

            foreach (var line in lines)
            {
                var parts = line.Split(":", 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim().Trim('"');
                    result.Meta[key] = value;
                }
            }
        }

        var templateMatch = Regex.Match(content, @"@Template\(""([\s\S]*?)""\)", RegexOptions.Multiline);
      
        if (templateMatch.Success)
        {
            result.Template = templateMatch.Groups[1].Value.Trim();
        }
        
        

        var paramsMatch = Regex.Match(content, @"@Params\s*\(\s*([\s\S]*?)\s*\)", RegexOptions.Multiline);
        if (paramsMatch.Success)
        {
            var block = paramsMatch.Groups[1].Value;
            var lineRegex = new Regex(@"^\s*([A-Za-z0-9_]+)\?\?(.*)\s*$", RegexOptions.Multiline);
            Console.WriteLine("Obtaining parameters from @Params...", ConsoleColor.Blue);

            foreach (Match m in lineRegex.Matches(block))
            {
                if(result.Params.ContainsKey(m.Groups[1].Value.Trim()))
                {
                    throw new ArgumentException(
                        $"There is a repeated param called {m.Groups[1].Value.Trim()}. Params names should be unique inside a template.");
                }
                var key = m.Groups[1].Value.Trim();
                var fallback = m.Groups[2].Value.Trim(); // "" si no hay fallback
                result.Params[key] = fallback;
                
                Console.WriteLine($"{key} => {fallback}", ConsoleColor.Blue);
            }
            Console.WriteLine();

        }
        else
        {
            Console.WriteLine("No @Params directive was found. The parameters from the @Template may not be asigned correctly", ConsoleColor.Yellow);
        }
        
        return result;
    }
}