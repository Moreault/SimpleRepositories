namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

public class IncludableGarbageBundle : IEntityBundle<IncludableGarbage>
{
    public IList<IncludableGarbage> Entities { get; set; } = new List<IncludableGarbage>();
}
