namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

public class GarbageBundleRepository : BundleRepository<Garbage, GarbageBundle>
{
    public static readonly IReadOnlyList<Garbage> Items = new List<Garbage>
    {
        new DerivedGarbage
        {
            Id = 4,
            Name = "Harry",
            Level = 7,
            Job = "That guy over there"
        },
        new()
        {
            Id = 1,
            Name = "Jerry",
            Level = 20
        },
        new()
        {
            Id = 3,
            Name = "Terry",
            Level = 18
        },
        new DerivedGarbage
        {
            Id = 2,
            Name = "Garry",
            Level = 45,
            Job = "Some guy"
        }
    };

    protected override int StartingId => 0;

    private GarbageBundle _bundle = new()
    {
        Entities = Items.ToList()
    };

    protected override Func<GarbageBundle> Load() => () => _bundle;

    protected override Action Commit(GarbageBundle bundle)
    {
        return () => _bundle = bundle;
    }

    protected override Garbage CreateEntityWithId(Garbage existing, int id) => existing with
    {
        Id = id
    };

    protected override GarbageBundle CreateBundle(IReadOnlyList<Garbage> entities) => new GarbageBundle
    {
        Entities = (IList<Garbage>)entities
    };
}