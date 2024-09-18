using System.Reflection;

namespace Application;

/// <summary>
/// References the application assembly.
/// </summary>
public static class ApplicationAssemblyReference
{
    /// <summary>
    /// Application assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}
