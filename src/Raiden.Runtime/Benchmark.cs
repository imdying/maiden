using System.Diagnostics;
using Humanizer;

namespace Raiden.Runtime;

/// <summary>
/// Provides a simple benchmark for assessing the runtime of an application.
/// </summary>
public static class Benchmark
{
    private static Stopwatch? _sw;

    /// <summary>
    /// Begins measuring.
    /// </summary>
    public static void Start()
    {
        AppDomain.CurrentDomain.ProcessExit += OnExit;
        _sw = Stopwatch.StartNew();
    }

    private static void OnExit(object? sender, EventArgs e)
        => Console.WriteLine($"\nRuntime elapsed: {ToHumanTime(_sw?.ElapsedMilliseconds)}");

    private static string ToHumanTime(long? ms) => TimeSpan.FromMilliseconds(ms ?? 0).Humanize(7, collectionSeparator: null);
}