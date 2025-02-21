using Application.Common.Persistence;

using SharedKernel.Models;
using SharedKernel.UnitTests.TestUtils.Extensions;

using Moq;

namespace Application.UnitTests.TestUtils.Extensions;

/// <summary>
/// Utilities for mocking EF core behaviors.
/// </summary>
public static class MockUnitOfWorkExtensions
{
    /// <summary>
    /// Mocks the SaveChangesAsync() method to set the id of an entity.
    /// </summary>
    /// <typeparam name="TRepository">The repository type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TEntityId">The entity type id.</typeparam>
    /// <param name="mockRepository">The repository mock.</param>
    /// <param name="mockUnitOfWork">The unit of work mock.</param>
    /// <param name="idToSet">The id to set in the entity after save changes.</param>
    public static void MockSetEntityIdBehavior<TRepository, TEntity, TEntityId>(
        this Mock<IUnitOfWork> mockUnitOfWork,
        Mock<TRepository> mockRepository,
        TEntityId idToSet
    )
        where TRepository : class, IBaseRepository<TEntity, TEntityId>
        where TEntity : AggregateRoot<TEntityId>
        where TEntityId : notnull
    {
        TEntity? capturedEntity = null;

        mockRepository
            .Setup(r => r.AddAsync(It.IsAny<TEntity>()))
            .Callback<TEntity>(entity => capturedEntity = entity);

        mockUnitOfWork
            .Setup(u => u.SaveChangesAsync())
            .Callback(() => capturedEntity!.SetIdUsingReflection(idToSet));
    }
}
