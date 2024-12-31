using Application.Common.Interfaces.Persistence;

using SharedKernel.Models;
using SharedKernel.UnitTests.TestUtils.Extensions;

using Moq;

namespace Application.UnitTests.TestUtils.Behaviors;

/// <summary>
/// Utilities for mocking EF core behaviors.
/// </summary>
public static class MockEFCoreBehaviors
{
    /// <summary>
    /// Mocks the SaveChangesAsync() method to set the id of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TEntityId">The entity type id.</typeparam>
    /// <param name="mockRepository">The mock repository.</param>
    /// <param name="mockUnitOfWork">The mock unit of work.</param>
    /// <param name="idToSet">The id that will be set in the entity after save changes is called.</param>
    public static void MockSetEntityIdBehavior<TEntity, TEntityId>(
        Mock<IRepository<TEntity, TEntityId>> mockRepository,
        Mock<IUnitOfWork> mockUnitOfWork,
        TEntityId idToSet
    )
        where TEntity : AggregateRoot<TEntityId>
        where TEntityId : class
    {
        TEntity? capturedEntity = null;

        mockRepository
           .Setup(r => r.AddAsync(It.IsAny<TEntity>()))
           .Callback<TEntity>(entity => capturedEntity = entity);

        mockUnitOfWork
           .Setup(u => u.SaveChangesAsync())
           .Callback(() =>
           {
               capturedEntity!.SetIdUsingReflection(idToSet);
           });
    }
}
