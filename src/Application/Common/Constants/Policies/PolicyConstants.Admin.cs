using Domain.UserAggregate.Entities;

namespace Application.Common.Constants.Policies;

public static partial class PolicyConstants
{
    /// <summary>
    /// Define the admin policy constants.
    /// </summary>
    public static class Admin
    {
        /// <summary>
        /// The policy name.
        /// </summary>
        public const string Name = "AdminPolicy";
        /// <summary>
        /// The policy role it requires.
        /// </summary>
        public static readonly string RoleName = Role.Admin.Name;
    }
    
}
