namespace ToolBX.SimpleRepositories;

/// <summary>
/// Identifies an optional, derived value that a repository can populate on fetched entities
/// (for example, resolving a <c>NameId</c> foreign key into a human-readable <c>Name</c>).
/// </summary>
/// <remarks>
/// This is a lightweight, AOT-friendly alternative to expression-based eager loading : repositories
/// interpret the requested includes in <c>ApplyIncludes</c> rather than through runtime reflection.
/// Consumers may declare their own includes simply by creating additional <see cref="Include"/> values.
/// </remarks>
public readonly record struct Include(string Key)
{
    public override string ToString() => Key;

    public static implicit operator Include(string key) => new(key);
}
