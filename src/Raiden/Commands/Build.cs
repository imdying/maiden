using Raiden.Data;
using Raiden.Runtime;
using Raiden.Runtime.Invocation;
using Raiden.SRLC;

namespace Raiden.Commands;

public sealed class Build : Command
{
    [Option(
        "--source",
        "A relative or absolute pathname of the root directory of your project.",
        Alias = "-s"
    )]
    public string Source { get; set; } = Directory.GetCurrentDirectory();

    private Configuration? Cfg
    {
        get;
        set;
    }

    private Release? Rl
    {
        get;
        set;
    }

    private Solution? Sln
    {
        get;
        set;
    }

    protected override CommandProperty Properties => new()
    {
        Description = "The powerhouse behind the tool, handling all versioning tasks."
    };

    protected override void Invocation(object?[] args)
    {
        Validate.ShouldNotBeNullOrEmpty(Source, nameof(Source));
        Validate.PathShouldExist(Source);

        Sln = new Solution(Source);
        Cfg = Sln.GetConfiguration();

        Validate.ShouldNotBeNullOrEmpty(
            Cfg.Script,
            nameof(Cfg.Script)
        );

        Validate.PathShouldExist(
            Path.GetFullPath(Cfg.Script!, Sln.Source.FullName)
        );

        // Versioning
        Rl = new Release(Cfg.Version);
        Rl.Invoke(Application.Theme);

        LoadScript(Sln, Cfg, Rl);
    }

    private static string GetRandomTmpFileName()
    {
        var path = Path.GetTempPath();
        var name = Path.GetRandomFileName();
        return Path.Combine(path, name);
    }

    private static string Replace(string content, Dictionary<string, string> symbols)
    {
        if (content.Length <= 0)
            throw new InvalidOperationException();

        foreach (var (Key, Value) in symbols)
        {
            content = content.Replace(Key, Value);
        }

        return content;
    }

    private void LoadScript(Solution sln, Configuration cfg, Release rl)
    {
        Benchmark.Start();
        // Directory.SetCurrentDirectory(Source);

        Script scriptCaller;
        Dictionary<string, string> symbols;

        // read resource from appdir
        var buildTemplate = Path.Combine(
            Application.GetPath(ApplicationDirectory.Scripts), 
            "buildInvoker.ps1"
        );

        var buildTemplateContent = File.ReadAllText(
            buildTemplate
        );

        symbols = new()
        {
            { "{{bScript}}", Path.GetFullPath(cfg.Script ?? string.Empty, sln.Source.FullName) },
            { "{{bNum}}", cfg.Build.Number.ToString() },
            { "{{bVer}}", cfg.Build.Version },
            { "{{bVerId}}", rl.Version.Stage.Name }
        };

        buildTemplateContent = Replace(
            buildTemplateContent,
            symbols
        );

        scriptCaller = new Script(
            string.Format("{0}.ps1", GetRandomTmpFileName())
        );

        // Saving modified script for later invocation.
        File.WriteAllText(
            scriptCaller.FileName, 
            buildTemplateContent
        );

        // Whether to save before or after successfully running the build-script.
        if (cfg.UpdateBeforeScript)
            scriptCaller.BeforeInvocation += OnUpdate;
        else
            scriptCaller.AfterInvocation += OnUpdate;

        #region Displaying Build Info
        Console.WriteLine();
        #endregion

        if (scriptCaller.Invoke() != 0)
        {
            Cli.WriteError(
                "Script has returned a non-zero exit code."
            );
        }

        #region Disposal
        try
        {
            File.Delete(
                scriptCaller.FileName
            );
        }
        catch (Exception)
        {

            Cli.WriteWarning(
                $"Failed to dispose of the temporary file at '{scriptCaller.FileName}'."
            );
        } 
        #endregion
    }

    private void OnUpdate(object? sender, EventArgs e)
    {
        if (Sln is null || Rl is null || Cfg is null)
        {
            throw new InvalidOperationException();
        }

        Rollback.Save(Sln, Cfg.Build);
        Cfg.Build.Update(Rl.Version.ToString());
        Cfg.Save();
    }
}