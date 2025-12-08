using System.Text.RegularExpressions;

namespace Scaffer.Core;

public static class TemplateEngine
{
   
    public static string Apply(string template, Dictionary<string,string> templateParams, Dictionary<string, string> cliParams)
    {
        Console.WriteLine();
        Console.WriteLine("Building template...");
        var declared = new Dictionary<string, string>();

        

        foreach (var kv in templateParams)
        {
            
            declared[kv.Key] = cliParams.TryGetValue(kv.Key, out string? cliParam) ? cliParam : kv.Value;    
            
        }

        template = Regex.Replace(template,
            @"\{\{\s*value\s*:\s*([A-Za-z0-9_]+)\s*\}\}",
            m =>
            {
                var key = m.Groups[1].Value;
                return declared.TryGetValue(key, out var val) ? val : m.Value;
            });

        

        return template;
    }
    


}