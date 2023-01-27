using System.Diagnostics;

namespace System;

public static class Extensions
{
    [DebuggerStepThrough]
    public static T ShouldNotBeNull<T>(this T? obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj), "Object failed null-check.");
        }

        return obj;
    }
}