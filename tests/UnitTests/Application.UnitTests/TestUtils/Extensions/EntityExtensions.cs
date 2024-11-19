using System.Reflection;
using SharedKernel.Models;

namespace Application.UnitTests.TestUtils.Extensions;

/// <summary>
/// Extension methods for the <see cref="Entity{TId}"/> class.
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Sets the Id property of an entity to the specified value, simulating what EF core does.
    /// </summary>
    /// <typeparam name="TId">The type of the entity id.</typeparam>
    /// <param name="entity">The entity to set the id.</param>
    /// <param name="id">The id to be set.</param>
    public static void SimulateEFSetIdBehavior<TId>(this Entity<TId> entity, TId id) where TId : class
    {
        var idProperty = typeof(Entity<TId>)
            .GetProperty(nameof(Entity<TId>.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?? throw new InvalidOperationException($"Could not set the id {id} for the entity");

        idProperty.SetValue(entity, id);
    }
}
