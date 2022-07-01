namespace SimpleRepositories.Bundles.Tests.Dummies;

public class DummyReadOnlyBundleRepository : ReadOnlyBundleRepository<Dummy, DummyBundle>
{
    public static readonly DummyBundle Items = new()
    {
        Entities = new List<Dummy>
        {
            new DerivedDummy
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
            new DerivedDummy
            {
                Id = 2,
                Name = "Garry",
                Level = 45,
                Job = "Some guy"
            }
        }
    };

    protected override Func<DummyBundle> Load() => () => Items;
}