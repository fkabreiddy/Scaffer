namespace Scaffer.Core.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteLineColor(string text, ConsoleColor color)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = prev;
        }
        
        public static string? GetArgValue(string[] args, params string[] keys)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (keys.Any(k =>
                        args[i].Equals(k, StringComparison.OrdinalIgnoreCase)))
                {
                    if (i + 1 < args.Length)
                    {
                        return args[i + 1];
                    }
                }
            }

            return null;
        }
        
        public static Dictionary<string, string> GetParams(string[] args, string argName)
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
        
    }
}