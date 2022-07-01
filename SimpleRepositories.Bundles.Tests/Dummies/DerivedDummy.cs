namespace SimpleRepositories.Bundles.Tests.Dummies;

public record DerivedDummy : Dummy
{
    public string Job { get; init; } = string.Empty;
}