using Domain.UserAggregate.Entities;

namespace Application.Common.Constants.Policies;

public static partial class PolicyConstants
{
    /// <summary>
    /// Define the customer policy constants.
    /// </summary>
    public static class Customer
    {
        /// <summary>
        /// The policy name.
        /// </summary>
        public const string Name = "CustomerPolicy";
        /// <summary>
        /// The role name the policy requires.
        /// </summary>
        public static readonly string RoleName = Role.Customer.Name;
    }
}
