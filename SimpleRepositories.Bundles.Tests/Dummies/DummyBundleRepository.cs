﻿namespace SimpleRepositories.Bundles.Tests.Dummies;

public class DummyBundleRepository : BundleRepository<Dummy, DummyBundle>
{
    public static readonly IReadOnlyList<Dummy> Items = new List<Dummy>
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
    };

    protected override int StartingId => 0;

    private DummyBundle _bundle = new()
    {
        Entities = Items.ToList()
    };

    protected override Func<DummyBundle> Load() => () => _bundle;

    protected override Action Commit(DummyBundle bundle)
    {
        return () => _bundle = bundle;
    }

    protected override Dummy CreateEntityWithId(Dummy existing, int id) => existing with
    {
        Id = id
    };

    protected override DummyBundle CreateBundle(IReadOnlyList<Dummy> entities) => new DummyBundle
    {
        Entities = (IList<Dummy>)entities
    };
}