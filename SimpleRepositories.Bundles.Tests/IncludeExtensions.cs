namespace SimpleRepositories.Bundles.Tests;

public static class IncludeExtensions
{
    extension(Include)
    {
        public static Include Name => new("Name");
        public static Include Description => new("Description");
    }
}