using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Define utilities for the <see cref="BaseEnumeration"/> class.
/// </summary>
public static class BaseEnumerationUtils
{
    /// <summary>
    /// Enumeration to create tests.
    /// </summary>
    public class SampleEnumeration : BaseEnumeration
    {
        /// <summary>
        /// Represents a first value.
        /// </summary>
        public static readonly SampleEnumeration First = new (1, "First");
        /// <summary>
        /// Represents a second value.
        /// </summary>
        public static readonly SampleEnumeration Second = new(2, "Second");

        /// <summary>
        /// Initiates a new instance of the <see cref="SampleEnumeration"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public SampleEnumeration(long id, string name) : base(id, name) { }
    }
}
