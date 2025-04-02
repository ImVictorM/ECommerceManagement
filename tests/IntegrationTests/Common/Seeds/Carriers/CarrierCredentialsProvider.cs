using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.DataTypes;

namespace IntegrationTests.Common.Seeds.Carriers;

/// <summary>
/// Represents a credentials provider for seed carriers.
/// </summary>
public class CarrierCredentialsProvider
    : CredentialsProvider<CarrierSeedType>, ICarrierCredentialsProvider
{
    private static readonly Dictionary<
        CarrierSeedType,
        AuthenticationCredentials
    > _credentials = new()
    {
        [CarrierSeedType.INTERNAL] = new AuthenticationCredentials(
            "carrier@email.com",
            "carrier123"
        ),
    };

    /// <summary>
    /// Initiates a new instance of the <see cref="CarrierCredentialsProvider"/>
    /// class.
    /// </summary>
    public CarrierCredentialsProvider() : base(_credentials)
    {
    }
}
