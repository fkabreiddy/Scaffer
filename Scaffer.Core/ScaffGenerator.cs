namespace Scaffer.Core;

public static class ScaffGenerator
{
    public static async Task Generate(ScaffFile scaff, Dictionary<string, string> parameters, string outputDir, string fullFileName)
    {
        if (!string.IsNullOrEmpty(outputDir) && outputDir != "./")
        {
            Directory.CreateDirectory(outputDir);
        }

        var result = TemplateEngine.Apply(scaff.Template, scaff.Params, parameters);

        
        var outputPath = Path.Combine(outputDir, fullFileName);
        await File.WriteAllTextAsync(outputPath, result);

        Console.WriteLine($"✅ File successfully created: {outputPath}");
    }
}