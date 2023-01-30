namespace Raiden.Environment
{
    internal static class SolutionExtensions
    {
        public static DirectoryInfo Combine(this DirectoryInfo dir, params string[] paths) 
            => new(Path.Combine(dir.FullName, Path.Combine(paths)));

        public static FileInfo Combine(this FileInfo file, params string[] paths)
            => new(Path.Combine(file.FullName, Path.Combine(paths)));

        public static DirectoryInfo CombineToDir(this FileInfo file, params string[] paths)
            => new(Path.Combine(file.FullName, Path.Combine(paths)));

        public static FileInfo CombineToFile(this DirectoryInfo dir, params string[] paths) 
            => new(Path.Combine(dir.FullName, Path.Combine(paths)));
    }
}