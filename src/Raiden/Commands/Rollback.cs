using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Raiden.Data;
using Raiden.SRLC.Terminal;

namespace Raiden.Commands;

public sealed class Rollback : Command
{
    private const string DAT_FILE = "rollback.dat";

    [Option(
        "--source",
        "A relative or absolute pathname of the root directory of your project.",
        Alias = "-s"
    )]
    public string Source { get; set; } = Directory.GetCurrentDirectory();

    // Todo: Add comment
    public static IReadOnlyList<ConfigurationMetadata> Fetch(Solution sln)
    {
        IReadOnlyList<ConfigurationMetadata>? result = null;
        var file = GetDataFile(sln);

        if (file.Exists)
        {
            result = Deserialize(
                File.ReadAllBytes(file.FullName)
            );
        }

        return result ?? Array.Empty<ConfigurationMetadata>();
    }

    // Todo: Add comment
    public static void Save(Solution sln, ConfigurationMetadata obj)
    {
        var file = GetDataFile(sln);
        var data = Fetch(sln).ToList();

        data.Add(obj);

        Serialize(
            file,
            data
        );
    }

    protected override CommandProperty Properties => new()
    {
        Description = "Roll back to a previous version."
    };

    protected override void Invocation(object?[] args)
    {
        Configuration cfg;
        ConfigurationMetadata result;
        IReadOnlyList<ConfigurationMetadata> data;
        string priorVersion;

        #region Checks
        Validate.ShouldNotBeNullOrEmpty(Source, nameof(Source));
        Validate.PathShouldExist(Source); 
        #endregion

        var sln = new Solution(Source);
        var dat = new Lazy<IReadOnlyList<ConfigurationMetadata>>(Fetch(sln));

        Directory.SetCurrentDirectory(sln.Source.FullName);

        #region Checks
        if (!sln.HasConfiguration)
        {
            Cli.WriteError("Solution has no configuration.");
            return;
        }

        if ((data = dat.Value).Count < 1)
        {
            Cli.WriteWarning(
                "Nothing to roll back to, except existence itself."
            );
            return;
        } 
        #endregion

        result = UserPrompt.PromptSelector(
            "Select a version to roll back to",
            data.Reverse(),
            Application.Theme
        );
        
        cfg = sln.GetConfiguration();

        // Changing values
        priorVersion = cfg.Version;
        cfg.Build = result;

        // Reconstruct the list
        data = data.TakeWhile(meta => meta != result).ToList();

        // Saving
        cfg.Save();
        Serialize(GetDataFile(sln), data);

        Console.WriteLine(
            $"Rolled back from '{priorVersion}' to '{result.Version}'."
        );
    }

    private static IReadOnlyList<ConfigurationMetadata>? Deserialize(byte[] data)
    {
        using var stream = new MemoryStream(data);
        using var reader = new BsonReader(stream)
        {
            ReadRootValueAsArray = true
        };

        var serializer = new JsonSerializer();
        var @object = serializer.Deserialize<IReadOnlyList<ConfigurationMetadata>>(reader);

        return @object;
    }

    private static FileInfo GetDataFile(Solution sln) => sln.Object.CombineToFile(DAT_FILE);
    
    private static void Serialize(FileInfo file, IEnumerable<ConfigurationMetadata> array)
    {
        using var stream = new MemoryStream();
        using var writer = new BsonWriter(stream);
        var serializer = new JsonSerializer();

        serializer.Serialize(writer, array);

        File.WriteAllBytes(
            file.FullName,
            stream.ToArray()
        );
    }
}