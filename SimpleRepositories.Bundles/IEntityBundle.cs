namespace ToolBX.SimpleRepositories.Bundles;

public interface IEntityBundle<T> where T : IAutoIncrementedId<int>
{
    IList<T> Entities { get; }
}