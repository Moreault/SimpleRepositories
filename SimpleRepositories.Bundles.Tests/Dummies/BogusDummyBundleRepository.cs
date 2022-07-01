namespace SimpleRepositories.Bundles.Tests.Dummies
{
    public class BogusDummyBundleRepository : DummyBundleRepository
    {
        protected override Dummy CreateEntityWithId(Dummy existing, int id) => existing with { Id = id - 1 };
    }
}
