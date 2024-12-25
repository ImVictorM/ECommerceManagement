using SharedKernel.Models;

namespace SharedKernel.UnitTests.TestUtils.Extensions;

/// <summary>
/// Extension methods for the <see cref="Entity{TId}"/> base class.
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Sets the id of an entity using reflection.
    /// </summary>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <param name="entity">The current entity.</param>
    /// <param name="id">The id to set.</param>
    /// <exception cref="InvalidOperationException">Thrown when it was not possible to get the entity id property.</exception>
    public static void SetIdUsingReflection<TId>(this Entity<TId> entity, TId id) where TId : notnull
    {
        var idProperty = typeof(Entity<TId>).GetProperty(
            nameof(entity.Id),
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
        );

        if (idProperty != null && idProperty.CanWrite)
        {
            idProperty.SetValue(entity, id);
        }
        else
        {
            throw new InvalidOperationException($"It was not possible to set the entity id");
        }
    }
}
