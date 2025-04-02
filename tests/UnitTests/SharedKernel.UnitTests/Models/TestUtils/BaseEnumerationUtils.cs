using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="BaseEnumeration"/> class.
/// </summary>
public static class BaseEnumerationUtils
{
    /// <summary>
    /// Represents a sample enumeration.
    /// </summary>
    public class SampleEnumeration : BaseEnumeration
    {
        /// <summary>
        /// Represents the first value.
        /// </summary>
        public static readonly SampleEnumeration First = new (1, nameof(First));
        /// <summary>
        /// Represents the second value.
        /// </summary>
        public static readonly SampleEnumeration Second = new(2, nameof(Second));

        /// <summary>
        /// Initiates a new instance of the <see cref="SampleEnumeration"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public SampleEnumeration(long id, string name) : base(id, name) { }
    }
}
