namespace ToolBX.SimpleRepositories.Bundles;

public interface IReadOnlyBundleRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IAutoIncrementedId<int>
{
    TEntity this[int id] { get; }

    TEntity FetchById(int id);
    TSubEntity FetchById<TSubEntity>(int id) where TSubEntity : TEntity?;

    TryGetResult<TEntity> TryFetchById(int id);
    TryGetResult<TSubEntity> TryFetchById<TSubEntity>(int id) where TSubEntity : TEntity?;

    IReadOnlyList<TEntity> FetchManyById(params int[] ids);
    IReadOnlyList<TEntity> FetchManyById(IEnumerable<int> ids);

    IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity?;
    IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(IEnumerable<int> ids) where TSubEntity : TEntity?;

    IReadOnlyList<TryGetResult<TEntity>> TryFetchManyById(params int[] ids);
    IReadOnlyList<TryGetResult<TEntity>> TryFetchManyById(IEnumerable<int> ids);

    IReadOnlyList<TryGetResult<TSubEntity>> TryFetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity?;
    IReadOnlyList<TryGetResult<TSubEntity>> TryFetchManyById<TSubEntity>(IEnumerable<int> ids) where TSubEntity : TEntity?;
}

public abstract class ReadOnlyBundleRepository<TEntity, TBundle> : IReadOnlyBundleRepository<TEntity> where TEntity : IAutoIncrementedId<int> where TBundle : IEntityBundle<TEntity>
{
    protected internal TBundle Bundle => _bundle.Value;
    private Lazy<TBundle> _bundle = null!;

    public TEntity this[int id] => FetchById(id);

    protected ReadOnlyBundleRepository()
    {
        Reset();
    }

    protected internal void Reset() => _bundle = new Lazy<TBundle>(() => Load().Invoke());

    protected abstract Func<TBundle> Load();

    public int Count() => Bundle.Entities.Count;

    public int Count(Func<TEntity, bool> predicate) => Bundle.Entities.Count(predicate);

    public int Count<TSubEntity>() where TSubEntity : TEntity? => Bundle.Entities.OfType<TSubEntity>().Count();

    public int Count<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity? => Bundle.Entities.OfType<TSubEntity>().Count(predicate);

    public IReadOnlyList<TEntity> FetchAll() => Bundle.Entities.OrderBy(x => x.Id).ToList();

    public IReadOnlyList<TSubEntity> FetchAll<TSubEntity>() where TSubEntity : TEntity? => Bundle.Entities.OfType<TSubEntity>().OrderBy(x => x!.Id).ToList();

    public IReadOnlyList<TEntity> FetchAll(Func<TEntity, bool> predicate) => Bundle.Entities.Where(predicate).ToList();

    public IReadOnlyList<TSubEntity> FetchAll<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity? => Bundle.Entities.OfType<TSubEntity>().Where(predicate).ToList();

    public TEntity Fetch(Func<TEntity, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var entity = Bundle.Entities.SingleOrDefault(predicate);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithPredicateNotFound, typeof(TEntity).GetHumanReadableName()));
        return entity;
    }

    public TSubEntity Fetch<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var entity = Bundle.Entities.OfType<TSubEntity>().SingleOrDefault(predicate);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithPredicateNotFound, typeof(TSubEntity).GetHumanReadableName()));
        return entity;
    }

    public TryGetResult<TEntity> TryFetch(Func<TEntity, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        try
        {
            return new TryGetResult<TEntity>(true, Fetch(predicate));
        }
        catch
        {
            return TryGetResult<TEntity>.Failure;
        }
    }

    public TryGetResult<TSubEntity> TryFetch<TSubEntity>(Func<TSubEntity, bool> predicate) where TSubEntity : TEntity?
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        try
        {
            return new TryGetResult<TSubEntity>(true, Fetch(predicate));
        }
        catch
        {
            return TryGetResult<TSubEntity>.Failure;
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

    public TEntity FetchById(int id)
    {
        var entity = Bundle.Entities.SingleOrDefault(x => x.Id == id);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithIdNotFound, typeof(TEntity).GetHumanReadableName(), id));
        return entity;
    }

    public TSubEntity FetchById<TSubEntity>(int id) where TSubEntity : TEntity?
    {
        var entity = Bundle.Entities.OfType<TSubEntity>().SingleOrDefault(x => x!.Id == id);
        if (entity == null) throw new Exception(string.Format(Exceptions.EntityWithIdNotFound, typeof(TSubEntity).GetHumanReadableName(), id));
        return entity;
    }

    public TryGetResult<TEntity> TryFetchById(int id)
    {
        try
        {
            return new TryGetResult<TEntity>(true, FetchById(id));
        }
        catch
        {
            return TryGetResult<TEntity>.Failure;
        }
    }

    public TryGetResult<TSubEntity> TryFetchById<TSubEntity>(int id) where TSubEntity : TEntity?
    {
        try
        {
            return new TryGetResult<TSubEntity>(true, FetchById<TSubEntity>(id));
        }
        catch
        {
            return TryGetResult<TSubEntity>.Failure;
        }
    }

    public IReadOnlyList<TEntity> FetchManyById(params int[] ids) => FetchManyById(ids as IEnumerable<int>);

    public IReadOnlyList<TEntity> FetchManyById(IEnumerable<int> ids)
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(FetchById).ToList();
    }

    public IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity?
    {
        return FetchManyById<TSubEntity>(ids as IEnumerable<int>);
    }

    public IReadOnlyList<TSubEntity> FetchManyById<TSubEntity>(IEnumerable<int> ids) where TSubEntity : TEntity?
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(FetchById<TSubEntity>).ToList();
    }

    public IReadOnlyList<TryGetResult<TEntity>> TryFetchManyById(params int[] ids) => TryFetchManyById(ids as IEnumerable<int>);

    public IReadOnlyList<TryGetResult<TEntity>> TryFetchManyById(IEnumerable<int> ids)
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(TryFetchById).ToList();
    }

    public IReadOnlyList<TryGetResult<TSubEntity>> TryFetchManyById<TSubEntity>(params int[] ids) where TSubEntity : TEntity? => TryFetchManyById<TSubEntity>(ids as IEnumerable<int>);

    public IReadOnlyList<TryGetResult<TSubEntity>> TryFetchManyById<TSubEntity>(IEnumerable<int> ids) where TSubEntity : TEntity?
    {
        if (ids == null) throw new ArgumentNullException(nameof(ids));
        return ids.Select(TryFetchById<TSubEntity>).ToList();
    }
}