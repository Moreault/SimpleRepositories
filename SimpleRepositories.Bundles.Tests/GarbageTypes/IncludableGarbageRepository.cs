namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

/// <summary>
/// A repository that resolves <see cref="IncludableGarbage.Name"/> from <see cref="IncludableGarbage.NameId"/>
/// only when <see cref="Include.Name"/> is requested.
/// </summary>
public class IncludableGarbageRepository : ReadOnlyBundleRepository<IncludableGarbage, IncludableGarbageBundle>
{
    public const string NamePrefix = "Name-";

    public static IncludableGarbageBundle CreateItems() => new()
    {
        Entities = new List<IncludableGarbage>
        {
            new() { Id = 1, NameId = 100 },
            new() { Id = 2, NameId = 200 },
            new() { Id = 3, NameId = 300 }
        }
    };

    private readonly IncludableGarbageBundle _bundle = CreateItems();

    protected override Func<IncludableGarbageBundle> Load() => () => _bundle;

    protected override void ApplyIncludes(IncludableGarbage entity, IReadOnlyList<Include> includes)
    {
        if (includes.Contains(Include.Name))
            entity.Name = $"{NamePrefix}{entity.NameId}";
    }
}

/// <summary>
/// A repository that does not override <c>ApplyIncludes</c>, used to verify that includes are a no-op by default.
/// </summary>
public class PlainIncludableGarbageRepository : ReadOnlyBundleRepository<IncludableGarbage, IncludableGarbageBundle>
{
    private readonly IncludableGarbageBundle _bundle = IncludableGarbageRepository.CreateItems();

    protected override Func<IncludableGarbageBundle> Load() => () => _bundle;
}
