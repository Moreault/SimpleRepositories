namespace ToolBX.SimpleRepositories;

public interface IReadOnlyRepository<TEntity>
{
    int Count();
    int Count(Func<TEntity, bool> predicate);
    int Count<TSubEntity>() where TSubEntity : TEntity?;
    int Count<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;

    IReadOnlyList<TEntity> FetchAll(params Include[] includes);
    IReadOnlyList<TSubEntity> FetchAll<TSubEntity>(params Include[] includes) where TSubEntity : TEntity?;

    IReadOnlyList<TEntity> FetchAll(Func<TEntity, bool> predicate, params Include[] includes);
    IReadOnlyList<TSubEntity> FetchAll<TSubEntity>(Func<TSubEntity, bool> predicate, params Include[] includes) where TSubEntity : TEntity?;

    TEntity Fetch(Func<TEntity, bool> predicate, params Include[] includes);
    TSubEntity Fetch<TSubEntity>(Func<TSubEntity, bool> predicate, params Include[] includes) where TSubEntity : TEntity?;

    Result<TEntity> TryFetch(Func<TEntity, bool> predicate, params Include[] includes);
    Result<TSubEntity> TryFetch<TSubEntity>(Func<TSubEntity, bool> predicate, params Include[] includes) where TSubEntity : TEntity?;

    bool Contains(params TEntity[] entities);
    bool Contains(IEnumerable<TEntity> entities);
    bool Contains(Func<TEntity, bool> predicate);
    bool Contains<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?;
}
