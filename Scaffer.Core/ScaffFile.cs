namespace Scaffer.Core;

public class ScaffFile
{
    public Dictionary<string, string> Params { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string Template { get; set; } = string.Empty;
    
}