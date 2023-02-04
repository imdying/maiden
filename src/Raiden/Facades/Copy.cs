using Maid.Sdk;

namespace Raiden.Facades;

internal sealed class Copy : Facade
{
    public event EventHandler? OnDuplication;

    public void SyncInvoke(string src, string dst, bool overwrite, bool quiet)
    {
        src = Path.GetFullPath(src);
        dst = Path.GetFullPath(dst);
        IsQuiet = quiet;

        if (string.IsNullOrWhiteSpace(src))
        {
            this.WriteError("Source cannot be empty or null.");
        }

        if (string.IsNullOrWhiteSpace(dst))
        {
            this.WriteError("Destination cannot be empty or null.");
        }

        if (!Path.Exists(src))
        {
            this.WriteError("Source doesn't exist.");
        }

        #region File Duplication
        if (IsFile(src))
        {
            Duplicate(new(src,
                          dst,
                          overwrite));
            return;
        }
        #endregion

        #region Directory Duplication
        try
        {
            this.WriteLine(string.Format("Source: {0}\nTarget: {1}\n", src, dst));

            BulkCopy(
                src, 
                dst, 
                overwrite
            );
        }
        catch (Exception e)
        {
            this.WriteError(e);
        }
        #endregion
    }

    private void BulkCopy(string src, string dst, bool overwrite)
    {
        var entries = Directory.GetFileSystemEntries(src, "*", SearchOption.AllDirectories);

        Parallel.ForEach(entries, entry =>
        {
            this.WriteLine($"Copying {entry}");
            Duplicate(new(entry, ConvertPath(entry, src, dst), overwrite));
        });
    }

    private void Duplicate(CopyContext context)
    {
        if (IsFile(context.Source))
        {
            var file = new FileInfo(context.Target);

            if (File.Exists(file.FullName) && !context.Overwrite)
                return;

            if (OnDuplication != null)
            {
                OnDuplication.Invoke(null, context);
            }

            if (!context.Handled)
            {
                if (file.Directory is not null)
                    Directory.CreateDirectory(file.Directory.FullName);

                File.Copy(context.Source, file.FullName, context.Overwrite);
            }
        }
        else
        {
            // Empty directory.
            Directory.CreateDirectory(context.Target);
        }
    }

    private static string ConvertPath(string path, string from, string to) => Path.Combine(to, Path.GetRelativePath(from, path));

    private static bool IsFile(string path) => !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
}