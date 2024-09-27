namespace SimpleRepositories.Bundles.Tests.GarbageTypes
{
    public class BogusGarbageBundleRepository : GarbageBundleRepository
    {
        protected override Garbage CreateEntityWithId(Garbage existing, int id) => existing with { Id = id - 1 };
    }
}
