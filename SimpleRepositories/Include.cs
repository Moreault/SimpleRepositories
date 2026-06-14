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
    /// <summary>A resolved display name (typically populated from a <c>NameId</c>).</summary>
    public static readonly Include Name = new(nameof(Name));

    /// <summary>A resolved description (typically populated from a <c>DescriptionId</c>).</summary>
    public static readonly Include Description = new(nameof(Description));

    public override string ToString() => Key;
}
