using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Constants;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace SharedKernel.UnitTests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="PasswordHash"/> value object.
/// </summary>
public class PasswordHashTests
{
    /// <summary>
    /// Verifies that a <see cref="PasswordHash"/> is created successfully with a valid hash and salt.
    /// </summary>
    [Fact]
    public void PasswordHash_WhenCreatingWithValidHashAndSalt_CreatesInstanceCorrectly()
    {
        var expectedHash = SharedKernelConstants.PasswordHash.Hash;
        var expectedSalt = SharedKernelConstants.PasswordHash.Salt;
        var passwordHash = PasswordHashUtils.Create(expectedHash, expectedSalt);

        passwordHash.Should().NotBeNull();
        passwordHash.Value.Should().Be($"{expectedHash}-{expectedSalt}");
    }

    /// <summary>
    /// Verifies that a <see cref="PasswordHash"/> is created successfully using a valid combined value.
    /// </summary>
    [Fact]
    public void PasswordHash_WhenCreatingWithValidValue_CreatesInstanceCorrectly()
    {
        var passwordHash = PasswordHashUtils.CreateUsingDirectValue(SharedKernelConstants.PasswordHash.Value);

        passwordHash.Should().NotBeNull();
        passwordHash.Value.Should().Be(SharedKernelConstants.PasswordHash.Value);
    }

    /// <summary>
    /// Verifies that the hash and salt parts of a <see cref="PasswordHash"/> can be retrieved individually.
    /// </summary>
    [Fact]
    public void PasswordHash_WhenGettingSaltAndHashPartsIndividually_ReturnsThemCorrectly()
    {
        var expectedHash = SharedKernelConstants.PasswordHash.Hash;
        var expectedSalt = SharedKernelConstants.PasswordHash.Salt;

        var passwordHash = PasswordHashUtils.Create(expectedHash, expectedSalt);

        passwordHash.GetHashPart().Should().Be(expectedHash);
        passwordHash.GetSaltPart().Should().Be(expectedSalt);
    }

    /// <summary>
    /// Verifies that creating a <see cref="PasswordHash"/> with invalid hexadecimal input throws a <see cref="DomainValidationException"/>.
    /// </summary>
    [Fact]
    public void PasswordHash_WhenCreatingWithInvalidHexInput_ThrowsError()
    {
        var invalidHash = "invalidHash";
        var invalidSalt = "invalidSalt";

        FluentActions
            .Invoking(() => PasswordHashUtils.Create(invalidHash, invalidSalt))
            .Should()
            .Throw<DomainValidationException>()
            .WithMessage("The hash or salt is not in a valid hexadecimal format");
    }

    /// <summary>
    /// Verifies that creating a <see cref="PasswordHash"/> with an invalid hash-salt template throws a <see cref="DomainValidationException"/>.
    /// </summary>
    [Fact]
    public void PasswordHash_WhenCreatingWithInvalidTemplate_ThrowsError()
    {
        var invalidPasswordHash = "invalidHashSaltFormat";

        FluentActions
            .Invoking(() => PasswordHashUtils.CreateUsingDirectValue(invalidPasswordHash))
            .Should()
            .Throw<DomainValidationException>()
            .WithMessage("Invalid hash and salt format");
    }
}
