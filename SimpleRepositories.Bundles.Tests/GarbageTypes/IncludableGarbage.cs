namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

/// <summary>
/// A mutable entity whose <see cref="Name"/> is a transient projection resolved from <see cref="NameId"/>
/// when an include is requested.
/// </summary>
public class IncludableGarbage : IAutoIncrementedId<int>
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public string? Name { get; set; }
}
