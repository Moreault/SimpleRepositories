namespace SimpleRepositories.Bundles.Tests.Dummies;

public class DummyBundle : IEntityBundle<Dummy>
{
    public IList<Dummy> Entities { get; set; } = new List<Dummy>();
}