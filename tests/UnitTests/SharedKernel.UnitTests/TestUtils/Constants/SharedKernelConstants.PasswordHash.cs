namespace SharedKernel.UnitTests.TestUtils.Constants;

public static partial class SharedKernelConstants
{
    /// <summary>
    /// Declare constants for the <see cref="PasswordHash"/> class.
    /// </summary>
    public static class PasswordHash
    {
        /// <summary>
        /// The hash constant.
        /// </summary>
        public const string Hash = "6333824CC074E187E261A0CBBD91F9741B4D38A26E1519A93B4244BEAFC933B9";
        /// <summary>
        /// The salt constant;
        /// </summary>
        public const string Salt = "4FDE231393F2C8AECC2B26F356E3D89E";
        /// <summary>
        /// The password hash value.
        /// </summary>
        public const string Value = $"{Hash}-{Salt}";
    }
}
