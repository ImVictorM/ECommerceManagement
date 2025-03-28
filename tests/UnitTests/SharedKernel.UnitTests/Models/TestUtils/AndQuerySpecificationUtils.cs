using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="AndQuerySpecification{T}"/> class.
/// </summary>
public static class AndQuerySpecificationUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="AndQuerySpecification{T}"/>
    /// class.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    /// <returns>
    /// A new instance of the <see cref="AndQuerySpecification{T}"/> class.
    /// </returns>
    public static AndQuerySpecification<T> CreateSpecification<T>(
        ISpecificationQuery<T> left,
        ISpecificationQuery<T> right
    ) where T : class
    {
        return new AndQuerySpecification<T>(left, right);
    }
}
