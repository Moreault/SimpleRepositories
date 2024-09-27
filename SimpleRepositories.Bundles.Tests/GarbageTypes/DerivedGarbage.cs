namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

public record DerivedGarbage : Garbage
{
    public string Job { get; init; } = string.Empty;
}