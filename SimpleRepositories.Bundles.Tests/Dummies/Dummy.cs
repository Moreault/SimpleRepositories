namespace SimpleRepositories.Bundles.Tests.Dummies;

public record Dummy : IAutoIncrementedId<int>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }
}