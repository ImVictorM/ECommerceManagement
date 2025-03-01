using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Carriers;

/// <summary>
/// Defines a contract to provide access to authentication credentials for carriers.
/// </summary>
public interface ICarrierCredentialsProvider : ICredentialsProvider<CarrierSeedType>
{
}
