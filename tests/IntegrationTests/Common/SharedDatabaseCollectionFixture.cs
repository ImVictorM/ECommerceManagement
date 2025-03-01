namespace IntegrationTests.Common;

/// <summary>
/// Defines a shared database context.
/// </summary>
[CollectionDefinition(nameof(SharedDatabaseCollectionFixture))]
public class SharedDatabaseCollectionFixture
    : ICollectionFixture<IntegrationTestWebAppFactory>
{
}
