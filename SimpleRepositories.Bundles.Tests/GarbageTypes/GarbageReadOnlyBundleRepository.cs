namespace SimpleRepositories.Bundles.Tests.GarbageTypes;

public class GarbageReadOnlyBundleRepository : ReadOnlyBundleRepository<Garbage, GarbageBundle>
{
    public static readonly GarbageBundle Items = new()
    {
        Entities = new List<Garbage>
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
        }
    };

    protected override Func<GarbageBundle> Load() => () => Items;
}