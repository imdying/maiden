namespace Maiden.Runtime
{
    public static class Extensions
    {
        public static T ShouldNotBeNull<T>(this T? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj;
        }
    }
}