using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace SharedKernel.UnitTests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="PasswordHash"/> value object.
/// </summary>
public class PasswordHashTests
{
    /// <summary>
    /// Verifies that a <see cref="PasswordHash"/> is created successfully
    /// with a valid hash and salt.
    /// </summary>
    [Fact]
    public void CreatePasswordHash_WithValidHashAndSalt_CreatesInstanceCorrectly()
    {
        var hashPart = PasswordHashUtils.GenerateRandomHash();
        var saltPart = PasswordHashUtils.GenerateRandomSalt();

        var actionResult = FluentActions
            .Invoking(() => PasswordHashUtils.Create(hashPart, saltPart))
            .Should()
            .NotThrow();

        var passwordHash = actionResult.Subject;

        passwordHash.Should().NotBeNull();
        passwordHash.Value.Should().Be($"{hashPart}-{saltPart}");
    }

    /// <summary>
    /// Verifies that a <see cref="PasswordHash"/> is created successfully
    /// using a valid combined value.
    /// </summary>
    [Fact]
    public void CreatePasswordHash_WithValidCombinedValue_CreatesInstanceCorrectly()
    {
        var hashPart = PasswordHashUtils.GenerateRandomHash();
        var saltPart = PasswordHashUtils.GenerateRandomSalt();

        var combinedValue = $"{hashPart}-{saltPart}";

        var actionResult = FluentActions
            .Invoking(() => PasswordHashUtils.CreateUsingDirectValue(combinedValue))
            .Should()
            .NotThrow();

        var passwordHash = actionResult.Subject;

        passwordHash.Should().NotBeNull();
        passwordHash.Value.Should().Be(combinedValue);
    }

    /// <summary>
    /// Verifies that creating a <see cref="PasswordHash"/> with an invalid
    /// hexadecimal input throws a <see cref="InvalidPatternException"/>.
    /// </summary>
    [Fact]
    public void CreatePasswordHash_WithInvalidHexInput_ThrowsError()
    {
        var invalidHash = "invalidHash";
        var invalidSalt = "invalidSalt";

        FluentActions
            .Invoking(() => PasswordHashUtils.Create(invalidHash, invalidSalt))
            .Should()
            .Throw<InvalidPatternException>()
            .WithMessage("The hash or salt is not in a valid hexadecimal format");
    }

    /// <summary>
    /// Verifies that creating a <see cref="PasswordHash"/> with an invalid hash-salt
    /// template throws a <see cref="InvalidPatternException"/>.
    /// </summary>
    [Fact]
    public void CreatePasswordHash_WithInvalidTemplate_ThrowsError()
    {
        var invalidPasswordHash = "invalidHashSaltFormat";

        FluentActions
            .Invoking(() =>
                PasswordHashUtils.CreateUsingDirectValue(invalidPasswordHash)
            )
            .Should()
            .Throw<InvalidPatternException>()
            .WithMessage("Invalid hash and salt template");
    }

    /// <summary>
    /// Verifies that the hash and salt parts of a <see cref="PasswordHash"/> can be
    /// retrieved individually.
    /// </summary>
    [Fact]
    public void GetParts_WithValidPasswordHash_ReturnsSaltAndHashPartsIndividually()
    {
        var hashPart = PasswordHashUtils.GenerateRandomHash();
        var saltPart = PasswordHashUtils.GenerateRandomSalt();

        var passwordHash = PasswordHashUtils.Create(hashPart, saltPart);

        passwordHash.GetHashPart().Should().Be(hashPart);
        passwordHash.GetSaltPart().Should().Be(saltPart);
    }
}
