namespace ToolBX.SimpleRepositories;

public interface IReadOnlyRepository<TEntity>
{
    int Count();
    int Count(Func<TEntity, bool> predicate);
    int Count<TSubEntity>() where TSubEntity : TEntity?;
    int Count<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;

    IReadOnlyList<TEntity> FetchAll();
    IReadOnlyList<TSubEntity> FetchAll<TSubEntity>() where TSubEntity : TEntity?;

    IReadOnlyList<TEntity> FetchAll(Func<TEntity, bool> predicate);
    IReadOnlyList<TSubEntity> FetchAll<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;

    TEntity Fetch(Func<TEntity, bool> predicate);
    TSubEntity Fetch<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;

    TryGetResult<TEntity> TryFetch(Func<TEntity, bool> predicate);
    TryGetResult<TSubEntity> TryFetch<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;

    bool Contains(params TEntity[] entities);
    bool Contains(IEnumerable<TEntity> entities);
    bool Contains(Func<TEntity, bool> predicate);
    bool Contains<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;
}