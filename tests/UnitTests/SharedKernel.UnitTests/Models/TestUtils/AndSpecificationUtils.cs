using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="AndSpecification{T}"/> class.
/// </summary>
public static class AndSpecificationUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="AndSpecification{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    /// <returns>A new instance of the <see cref="AndSpecification{T}"/> class.</returns>
    public static AndSpecification<T> CreateSpec<T>(ISpecification<T> left, ISpecification<T> right) where T : class
    {
        return new AndSpecification<T>(left, right);
    }
}
