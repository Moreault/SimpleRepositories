namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

public class GarbageBundle : IEntityBundle<Garbage>
{
    public IList<Garbage> Entities { get; set; } = new List<Garbage>();
}