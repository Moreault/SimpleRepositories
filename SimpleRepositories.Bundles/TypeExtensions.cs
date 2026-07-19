namespace ToolBX.SimpleRepositories.Bundles;

internal static class TypeExtensions
{
    /// <summary>
    /// Returns a readable type name with generic arguments spelled out, e.g. <c>Dictionary&lt;String, Int32&gt;</c>.
    /// Reads only <see cref="Type.Name"/> and <see cref="Type.GetGenericArguments"/>, so it is trimming/Native AOT safe.
    /// </summary>
    public static string GetHumanReadableName(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var name = type.Name;
        var indexOfApostrophe = name.IndexOf('`');
        if (indexOfApostrophe <= -1) return name;

        var generics = type.GetGenericArguments().Select(x => x.GetHumanReadableName());
        return $"{name[..indexOfApostrophe]}<{string.Join(", ", generics)}>";
    }
}
