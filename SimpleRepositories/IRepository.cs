namespace ToolBX.SimpleRepositories;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
{
    void Update(TEntity entity);
    void UpdateMany(params TEntity[] entities);
    void UpdateMany(IEnumerable<TEntity> entities);

    TEntity Insert(TEntity entity);
    IReadOnlyList<TEntity> InsertMany(params TEntity[] entities);
    IReadOnlyList<TEntity> InsertMany(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
    void Delete(Func<TEntity, bool> predicate);

    void TryDelete(TEntity entity);
    void TryDelete(Func<TEntity, bool> predicate);

    void DeleteMany(params TEntity[] entities);
    void DeleteMany(IEnumerable<TEntity> entities);

    void TryDeleteMany(params TEntity[] entities);
    void TryDeleteMany(IEnumerable<TEntity> entities);
}