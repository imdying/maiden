using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Raiden.Data;

public sealed partial class Template
{
    private static readonly string _basePath = Application.GetPath(ApplicationDirectory.Solutions);
    private static readonly string _fileName = "raiden.template.jsonc";

    public Template(FileInfo src, List<string>? ignore)
    {
        Source = src;
        Ignore = ignore;
    }

    public string Directory => Source.DirectoryName ?? string.Empty;

    public List<string>? Ignore
    {
        get;
        set;
    }

    public FileInfo Source
    {
        get;
        init;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the template was found, else <see langword="false"/>.
    /// </summary>
    public static bool GetTemplate(string name, out Template? template)
    {
        template = null;
        var templates = GetTemplates();

        if (templates.TryGetValue(name, out var templateDir))
        {
            template = Parse(
                Path.Combine(templateDir, _fileName)
            );
            return true;
        }

        return false;
    }

    public static Dictionary<string, string> GetTemplates()
    {
        var templates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var templateBasePath = new DirectoryInfo(_basePath);

        if (!templateBasePath.Exists)
        {
            throw new FileNotFoundException(_basePath);
        }

        foreach (var dir in templateBasePath.GetDirectories())
        {
            if (File.Exists(Path.Combine(dir.FullName, _fileName)))
            {
                templates.Add(Path.GetFileName(dir.FullName), dir.FullName);
            }
        }

        return templates;
    }

    public bool IsIgnore(string fileName)
    {
        Match expression;

        if (fileName is null)
        {
            return false;
        }

        for (int i = 0; i < Ignore?.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(Ignore[i]))
                return false;

            // If the value is to be interpreted as a regex expression.
            if ((expression = RegexIndentifier().Match(Ignore[i])).Success)
            {
                if (Regex.Match(fileName, expression.Groups[^1].Value, RegexOptions.Singleline).Success)
                    return true;
            }

            if (Path.GetFullPath(Ignore[i], Directory) == fileName)
            {
                return true;
            }
        }

        return false;
    }

    private static Template Parse(string path)
    {
        var file = new FileInfo(path);
        var fileContent = File.ReadAllText(file.FullName);

        // deserializing
        var template = JsonConvert.DeserializeObject<Template>(
            fileContent,
            new JsonSerializerSettings()
            {
                Converters = new[]
                {
                    new TemplateDeserializer(file)
                }
            }
        );

        ArgumentNullException.ThrowIfNull(template);

        // Additional computation before returning.
        (template.Ignore ??= new()).Add(file.FullName);

        return template;
    }

    [GeneratedRegex("^(Regex:\\/\\/)(.+)")]
    private static partial Regex RegexIndentifier();
}