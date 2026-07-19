![SimpleRepositories](https://github.com/Moreault/SimpleRepositories/blob/master/simplerepositories.png)
# SimpleRepositories
Base classes and interfaces for setting up simple repositories.

## Bundles

### What the hell is a bundle?
A bundle is basically just a *bundle* of entities which is typically stored in a serializable file that you can query and/or save to.

It's useful if you need to store data but don't necessarily require a database or if you're just prototyping a quick and dirty idea. 

For your information, I use ReadOnlyBundleRepository and BundleRepository to store Rough Trigger's game data such as spritesheet maps (holds sprite positions and sizes,) tilesets or items.

### Does it have to be a serialized file?
No. It can be just about anything, really. One of the only contraint for bundles is that entities must have an Id property. This id is automatically incremented whenever a new entity is added to the bundle using ToolBX.Unicity.

### Getting started
Check out the unit test project for samples on how to use it.

### StartingId
By default, the starting ID for a bundle is 0. If you want to change that, you can do so by overriding the StartingId property in your bundle repository.

```cs
public class MyBundleRepository : BundleRepository<MyEntity>
{
	protected override int StartingId => 1;
}
```

With this, your items will be assigned IDs starting at 1 instead of 0.

### Includes

Entities often store foreign keys (such as a `NameId` or `DescriptionId` pointing into a localization table) rather than the resolved value itself. Includes let a repository optionally populate those derived values when you fetch, similar in spirit to EF Core's `Include` but deliberately lightweight and AOT-friendly: there are no expression trees or runtime reflection involved.

Includes are entirely opt-in. Fetched entities are only enriched when the caller asks for it *and* the repository knows how to resolve it by overriding `ApplyIncludes`. If you request an include a repository doesn't handle, it's simply ignored.

```cs
public class ItemRepository : BundleRepository<Item, ItemBundle>
{
    private readonly ITextRepository _text;

    public ItemRepository(ITextRepository text) => _text = text;

    protected override void ApplyIncludes(Item entity, IReadOnlyList<Include> includes)
    {
        if (includes.Contains(Include.Name))
            entity.Name = _text.Fetch(entity.NameId);
        if (includes.Contains(Include.Description))
            entity.Description = _text.Fetch(entity.DescriptionId);
    }
}
```

```cs
var bare = repository.FetchById(5);                                  // Name/Description left untouched
var named = repository.FetchById(5, Include.Name);                  // Name resolved
var full = repository.FetchAll(Include.Name, Include.Description);   // both resolved for every entity
```

You may add extension properties to `Include`:

```cs
public static class IncludeExtensions
{
    extension(Include)
    {
        public static Include Name => new("Name");
        public static Include Description => new("Description");
        public static Include ObjectiveText => new("ObjectiveText");
    }
}
```

Note that includes populate the entity in place, so if your repository caches its bundle in memory the resolved values will persist on the cached instances. Clone inside `ApplyIncludes` if you need isolation.