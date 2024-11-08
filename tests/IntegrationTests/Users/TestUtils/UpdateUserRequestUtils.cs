using Contracts.Users;
using Domain.UnitTests.TestUtils.Constants;

namespace IntegrationTests.Users.TestUtils;

public static class UpdateUserRequestUtils
{
    public static UpdateUserRequest CreateRequest(
        string? name = null,
        string? phone = null,
        string? email = null
    )
    {
        return new UpdateUserRequest(
            name ?? DomainConstants.User.Name,
            phone,
            email ?? DomainConstants.Email.Value
        );
    }
}
