namespace ToolBX.SimpleRepositories.Bundles;

public interface IReadOnlyBundleRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IAutoIncrementedId<int>
{
    TEntity this[int id] { get; }

    TEntity FetchById(int id, params Include[] includes);
    TSubEntity FetchById<TSubEntity>(int id, params Include[] includes) where TSubEntity : TEntity?;

    Result<TEntity> TryFetchById(int id, params Include[] includes);
    Result<TSubEntity> TryFetchById<TSubEntity>(int id, params Include[] includes) where TSubEntity : TEntity?;

    IReadOnlyList<TEntity> FetchManyById(params int[] ids);
    IReadOnlyList<TEntity> FetchManyById(IEnumerable<int> ids, params Include[] includes);

    IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity?;
    IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(IEnumerable<int> ids, params Include[] includes) where TSubEntity : TEntity?;

    IReadOnlyList<Result<TEntity>> TryFetchManyById(params int[] ids);
    IReadOnlyList<Result<TEntity>> TryFetchManyById(IEnumerable<int> ids, params Include[] includes);

    IReadOnlyList<Result<TSubEntity>> TryFetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity?;
    IReadOnlyList<Result<TSubEntity>> TryFetchManyById<TSubEntity>(IEnumerable<int> ids, params Include[] includes) where TSubEntity : TEntity?;
}

public abstract class ReadOnlyBundleRepository<TEntity, TBundle> : IReadOnlyBundleRepository<TEntity> where TEntity : IAutoIncrementedId<int> where TBundle : IEntityBundle<TEntity>
{
    protected internal TBundle Bundle
    {
        get
        {
            if (!_isBundleLoaded)
                lock (_bundleLock)
                    if (!_isBundleLoaded)
                    {
                        _bundle = Load().Invoke();
                        _isBundleLoaded = true;
                    }
            return _bundle;
        }
    }
    private TBundle _bundle = default!;
    private bool _isBundleLoaded;
    private readonly object _bundleLock = new();

    public TEntity this[int id] => FetchById(id);

    protected ReadOnlyBundleRepository()
    {
        Reset();
    }

    protected internal void Reset()
    {
        lock (_bundleLock)
        {
            _bundle = default!;
            _isBundleLoaded = false;
        }
    }

    protected abstract Func<TBundle> Load();

    /// <summary>
    /// Populates optional, derived values on a freshly fetched entity. The default implementation does nothing,
    /// so includes are entirely opt-in : an entity is only enriched when the caller requests it and a derived
    /// repository overrides this method. Override it to resolve foreign keys into navigation values
    /// (for example, set <c>Name</c> from <c>NameId</c> when <paramref name="includes"/> contains <see cref="Include.Name"/>).
    /// </summary>
    /// <remarks>This runs after every fetch and is expected to mutate <paramref name="entity"/> in place.</remarks>
    protected virtual void ApplyIncludes(TEntity entity, IReadOnlyList<Include> includes) { }

    private TResult Materialize<TResult>(TResult entity, Include[] includes) where TResult : TEntity?
    {
        if (includes.Length > 0 && entity is not null) ApplyIncludes(entity, includes);
        return entity;
    }

    private IReadOnlyList<TResult> MaterializeAll<TResult>(IReadOnlyList<TResult> entities, Include[] includes) where TResult : TEntity?
    {
        if (includes.Length > 0)
            foreach (var entity in entities)
                if (entity is not null)
                    ApplyIncludes(entity, includes);
        return entities;
    }

    public int Count() => Bundle.Entities.Count;

    public int Count(Func<TEntity, bool> predicate) => Bundle.Entities.Count(predicate);

    public int Count<TSubEntity>() where TSubEntity : TEntity? => Bundle.Entities.OfType<TSubEntity>().Count();

    public int Count<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity? => Bundle.Entities.OfType<TSubEntity>().Count(predicate);

    public IReadOnlyList<TEntity> FetchAll(params Include[] includes) => MaterializeAll(Bundle.Entities.OrderBy(x => x.Id).ToList(), includes);

    public IReadOnlyList<TSubEntity> FetchAll<TSubEntity>(params Include[] includes) where TSubEntity : TEntity? => MaterializeAll(Bundle.Entities.OfType<TSubEntity>().OrderBy(x => x!.Id).ToList(), includes);

    public IReadOnlyList<TEntity> FetchAll(Func<TEntity, bool> predicate, params Include[] includes) => MaterializeAll(Bundle.Entities.Where(predicate).ToList(), includes);

    public IReadOnlyList<TSubEntity> FetchAll<TSubEntity>(Func<TSubEntity, bool> predicate, params Include[] includes) where TSubEntity : TEntity? => MaterializeAll(Bundle.Entities.OfType<TSubEntity>().Where(predicate).ToList(), includes);

    public TEntity Fetch(Func<TEntity, bool> predicate, params Include[] includes)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var entity = Bundle.Entities.SingleOrDefault(predicate);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithPredicateNotFound, typeof(TEntity).GetHumanReadableName()));
        return Materialize(entity, includes);
    }

    public TSubEntity Fetch<TSubEntity>(Func<TSubEntity, bool> predicate, params Include[] includes) where TSubEntity : TEntity?
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var entity = Bundle.Entities.OfType<TSubEntity>().SingleOrDefault(predicate);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithPredicateNotFound, typeof(TSubEntity).GetHumanReadableName()));
        return Materialize(entity, includes);
    }

    public Result<TEntity> TryFetch(Func<TEntity, bool> predicate, params Include[] includes)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        try
        {
            return Result<TEntity>.Success(Fetch(predicate, includes));
        }
        catch
        {
            return Result<TEntity>.Failure();
        }
    }

    public Result<TSubEntity> TryFetch<TSubEntity>(Func<TSubEntity, bool> predicate, params Include[] includes) where TSubEntity : TEntity?
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        try
        {
            return Result<TSubEntity>.Success(Fetch(predicate, includes));
        }
        catch
        {
            return Result<TSubEntity>.Failure();
        }
    }

    public bool Contains(params TEntity[] entities) => Contains(entities as IEnumerable<TEntity>);

    public bool Contains(IEnumerable<TEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));
        return entities.All(entity => Bundle.Entities.Any(x => Equals(x, entity)));
    }

    public bool Contains(Func<TEntity, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return Bundle.Entities.Any(predicate);
    }

    public bool Contains<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return Bundle.Entities.OfType<TSubEntity>().Any(predicate);
    }

    public TEntity FetchById(int id, params Include[] includes)
    {
        var entity = Bundle.Entities.SingleOrDefault(x => x.Id == id);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithIdNotFound, typeof(TEntity).GetHumanReadableName(), id));
        return Materialize(entity, includes);
    }

    public TSubEntity FetchById<TSubEntity>(int id, params Include[] includes) where TSubEntity : TEntity?
    {
        var entity = Bundle.Entities.OfType<TSubEntity>().SingleOrDefault(x => x!.Id == id);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithIdNotFound, typeof(TSubEntity).GetHumanReadableName(), id));
        return Materialize(entity, includes);
    }

    public Result<TEntity> TryFetchById(int id, params Include[] includes)
    {
        try
        {
            return Result<TEntity>.Success(FetchById(id, includes));
        }
        catch
        {
            return Result<TEntity>.Failure();
        }
    }

    public Result<TSubEntity> TryFetchById<TSubEntity>(int id, params Include[] includes) where TSubEntity : TEntity?
    {
        try
        {
            return Result<TSubEntity>.Success(FetchById<TSubEntity>(id, includes));
        }
        catch
        {
            return Result<TSubEntity>.Failure();
        }
    }

    public IReadOnlyList<TEntity> FetchManyById(params int[] ids) => FetchManyById(ids as IEnumerable<int>);

    public IReadOnlyList<TEntity> FetchManyById(IEnumerable<int> ids, params Include[] includes)
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(id => FetchById(id, includes)).ToList();
    }

    public IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity?
    {
        return FetchManyById<TSubEntity>(ids as IEnumerable<int>);
    }

    public IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(IEnumerable<int> ids, params Include[] includes) where TSubEntity : TEntity?
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(id => FetchById<TSubEntity>(id, includes)).ToList();
    }

    public IReadOnlyList<Result<TEntity>> TryFetchManyById(params int[] ids) => TryFetchManyById(ids as IEnumerable<int>);

    public IReadOnlyList<Result<TEntity>> TryFetchManyById(IEnumerable<int> ids, params Include[] includes)
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(id => TryFetchById(id, includes)).ToList();
    }

    public IReadOnlyList<Result<TSubEntity>> TryFetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity? => TryFetchManyById<TSubEntity>(ids as IEnumerable<int>);

    public IReadOnlyList<Result<TSubEntity>> TryFetchManyById<TSubEntity>(IEnumerable<int> ids, params Include[] includes) where TSubEntity : TEntity?
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(id => TryFetchById<TSubEntity>(id, includes)).ToList();
    }
}