namespace IntegrationTests.Common.Seeds.Users;

/// <summary>
/// Represents the types of users available in the database seed.
/// </summary>
public enum UserSeedType
{
    /// <summary>
    /// Represents an admin user.
    /// </summary>
    ADMIN,
    /// <summary>
    /// Represents other admin user.
    /// </summary>
    OTHER_ADMIN,
    /// <summary>
    /// Represents a customer user.
    /// </summary>
    CUSTOMER,
    /// <summary>
    /// Represents a customer with address.
    /// </summary>
    CUSTOMER_WITH_ADDRESS,
    /// <summary>
    /// Represents an inactive customer.
    /// </summary>
    CUSTOMER_INACTIVE,
}

