namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

public record Garbage : IAutoIncrementedId<int>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }
}