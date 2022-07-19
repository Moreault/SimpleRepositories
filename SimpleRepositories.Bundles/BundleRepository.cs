namespace ToolBX.SimpleRepositories.Bundles;

public interface IBundleRepository<TEntity> : IReadOnlyBundleRepository<TEntity>, IRepository<TEntity> where TEntity : IAutoIncrementedId<int>
{
    void DeleteById(int id);
    void DeleteManyById(params int[] ids);
    void DeleteManyById(IEnumerable<int> ids);

    void TryDeleteById(int id);
    void TryDeleteManyById(params int[] ids);
    void TryDeleteManyById(IEnumerable<int> ids);
}

public abstract class BundleRepository<TEntity, TBundle> : ReadOnlyBundleRepository<TEntity, TBundle>, IBundleRepository<TEntity> where TEntity : IAutoIncrementedId<int> where TBundle : IEntityBundle<TEntity>
{
    protected abstract Action Commit(IList<TEntity> entities);

    protected abstract TEntity CreateEntityWithId(TEntity existing, int id);

    public void Update(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        UpdateMany(entity);
    }

    public void UpdateMany(params TEntity[] entities) => UpdateMany(entities as IEnumerable<TEntity>);

    public void UpdateMany(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));
        var list = entities as IReadOnlyList<TEntity> ?? entities.ToList();
        if (!list.Any()) throw new ArgumentException(string.Format(Exceptions.NoEntityToUpdate, typeof(TEntity).GetHumanReadableName()));
        if (list.Any(x => x is null)) throw new ArgumentException(string.Format(Exceptions.TryingToUpdateNulls, typeof(TEntity).GetHumanReadableName()));

        var bundle = Bundle.Entities.ToList();

        foreach (var entity in list)
        {
            var existingIndex = bundle.IndexesOf(x => x.Id == entity.Id);
            if (existingIndex.IsNullOrEmpty())
                throw new Exception(string.Format(Exceptions.NoEntityFoundToUpdate, typeof(TEntity).GetHumanReadableName(), entity.Id));
            bundle[existingIndex.Single()] = entity;
        }

        Commit(bundle).Invoke();
        AfterUpdate(list);
        Reset();
    }

    protected virtual void AfterUpdate(IReadOnlyList<TEntity> entities)
    {

    }

    public TEntity Insert(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return InsertMany(entity).Single();
    }

    public IReadOnlyList<TEntity> InsertMany(params TEntity[] entities) => InsertMany(entities as IEnumerable<TEntity>);

    public IReadOnlyList<TEntity> InsertMany(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));
        var list = entities as IReadOnlyList<TEntity> ?? entities.ToList();
        if (!list.Any()) throw new ArgumentException(string.Format(Exceptions.NoEntityToInsert, typeof(TEntity).GetHumanReadableName()));
        if (list.Any(x => x is null)) throw new ArgumentException(string.Format(Exceptions.TryingToInsertNulls, typeof(TEntity).GetHumanReadableName()));

        var bundle = Bundle.Entities.ToList();

        var output = new List<TEntity>();
        foreach (var entity in list)
        {
            var id = bundle.Cast<IAutoIncrementedId<int>>().ToList().GetNextAvailableId();
            var newEntity = CreateEntityWithId(entity, id);

            if (newEntity.Id != id)
                throw new Exception(string.Format(Exceptions.IdWasChangedBeforeInsert, typeof(TEntity).GetHumanReadableName(), id, newEntity.Id));

            bundle.Add(newEntity);
            output.Add(newEntity);
        }
        Commit(bundle).Invoke();
        AfterInsert(list);
        Reset();

        return output;
    }

    protected virtual void AfterInsert(IReadOnlyList<TEntity> entities)
    {

    }

    public void Delete(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        Delete(x => Equals(x, entity));
    }

    public void Delete(Func<TEntity, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var entities = FetchAll(predicate);
        if (!entities.Any()) throw new Exception(string.Format(Exceptions.TryingToDeleteInexistantEntities, typeof(TEntity).GetHumanReadableName()));
        DeleteMany(entities);
    }

    public void TryDelete(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        try
        {
            Delete(entity);
        }
        catch
        {
            // ignored
        }
    }

    public void TryDelete(Func<TEntity, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        try
        {
            Delete(predicate);
        }
        catch
        {
            // ignored
        }
    }

    protected virtual void AfterDelete(IReadOnlyList<TEntity> entities)
    {

    }

    public void DeleteMany(params TEntity[] entities)
    {
        DeleteMany(entities as IEnumerable<TEntity>);
    }

    public void DeleteMany(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        var list = entities.ToList();
        if (entities.IsNullOrEmpty()) throw new ArgumentException(string.Format(Exceptions.NoEntityToDelete, typeof(TEntity).GetHumanReadableName()));
        if (list.Any(x => x is null)) throw new ArgumentException(string.Format(Exceptions.TryingToDeleteNulls, typeof(TEntity).GetHumanReadableName()));

        var bundle = Bundle.Entities.ToList();
        foreach (var entity in list)
        {
            if (!Contains(entity)) throw new Exception(string.Format(Exceptions.TryingToDeleteInexistantEntities, typeof(TEntity).GetHumanReadableName()));
            bundle.Remove(entity);
        }

        Commit(bundle).Invoke();
        AfterDelete(list);
        Reset();
    }

    public void TryDeleteMany(params TEntity[] entities)
    {
        if (entities.IsNullOrEmpty()) throw new ArgumentException(string.Format(Exceptions.NoEntityToDelete, typeof(TEntity).GetHumanReadableName()));
        try
        {
            TryDeleteMany(entities as IEnumerable<TEntity>);
        }
        catch
        {
            // ignored
        }
    }

    public void TryDeleteMany(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));
        entities = entities.Where(x => Contains(x));
        try
        {
            DeleteMany(entities);
        }
        catch
        {
            // ignored
        }
    }

    public void DeleteById(int id) => DeleteManyById(id);

    public void DeleteManyById(params int[] ids) => DeleteManyById(ids as IEnumerable<int>);

    public void DeleteManyById(IEnumerable<int> ids)
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));

        var list = ids.ToList();
        if (list.Any(x => !Contains(y => x == y.Id))) throw new Exception(string.Format(Exceptions.TryingToDeleteInexistantEntities, typeof(TEntity).GetHumanReadableName()));

        Delete(x => list.Contains(x.Id));
    }

    public void TryDeleteById(int id)
    {
        try
        {
            DeleteById(id);
        }
        catch
        {
            // ignored
        }
    }

    public void TryDeleteManyById(params int[] ids)
    {
        try
        {
            TryDeleteManyById(ids as IEnumerable<int>);
        }
        catch
        {
            // ignored
        }
    }

    public void TryDeleteManyById(IEnumerable<int> ids)
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        ids = ids.Where(x => Contains(y => x == y.Id));
        try
        {
            DeleteManyById(ids);
        }
        catch
        {
            // ignored
        }
    }
}