using Raiden.Data;
using Raiden.Facades;
using Raiden.Runtime.Invocation;

namespace Raiden.Commands;

public sealed class New : Command
{
    private string? _name;
    private string? _templateName;

    [Option(
        "--name",
        "The name of the solution. Defaults to the name of the output directory.",
        Alias = "-n"
    )]
    public string Name
    {
        get => _name ??= (Path.GetFileName(Output) ?? "Undefined");
        set => _name = value;
    }

    [Option(
        "--output", 
        "Location to place the generated output.", 
        Alias = "-o"
    )]
    public string Output { get; set; } = Directory.GetCurrentDirectory();

    [Argument(
        1, 
        "Template", 
        "The name of the template.", 
        Arity = ArgumentArity.ZeroOrOne
    )]
    public string TemplateName
    {
        get => _templateName ??= "Artifacts";
        set => _templateName = value;
    }

    protected override CommandProperty Properties => new()
    {
        Description = "Create a ready-to-go project structure for your .NET projects."
    };

    protected override void Invocation(object?[] args)
    {
        Template template;

        #region Checks
        Validate.ShouldNotBeNullOrEmpty(Output, nameof(Output));
        Validate.ShouldNotBeNullOrEmpty(Name, nameof(Name));
        Validate.ShouldNotBeNullOrEmpty(TemplateName, nameof(TemplateName));

        if (Output.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            Cli.WriteError("Output cannot contain any illegal characters.");

        if (Name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            Cli.WriteError("Name cannot contain any illegal characters.");

        if (File.Exists($"{Path.Combine(Output, Name)}.sln"))
            Cli.WriteError("A solution already exists in the directory.");

        if (!Template.GetTemplate(TemplateName, out template!))
            Cli.WriteError($"No templates found matching: '{TemplateName}'.");
        #endregion

        #region Solution
        var dotnetExitResult = new Process("dotnet").CreateProcess(
            $"new sln -v q -n \"{Name}\" -o \"{Output}\" --no-update-check"
        );

        if (dotnetExitResult != 0)
        {
            Cli.WriteError("An error occured while creating solution.");
        }
        #endregion

        #region Template
        var copyFacade = new Copy();

        copyFacade.OnDuplication += (s, e) =>
        {
            var context = (CopyContext)e;

            if (template.IsIgnore(context.Source))
            {
                context.Handled = true;
            }
        };

        copyFacade.SyncInvoke(
            template.Directory,
            Output,
            false,
            true
        );
        #endregion

        Console.WriteLine($"The template \"{TemplateName}\" was created successfully.");
    }
}