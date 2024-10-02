using Domain.Common.Models;

namespace Domain.UnitTests.Common.TestUtils;

/// <summary>
/// Value object utilities.
/// </summary>
public static class ValueObjectUtils
{
    private class EmailValueObject : ValueObject
    {
        public string Value { get; }

        public EmailValueObject(string email)
        {
            Value = email;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    private class PasswordValueObject : ValueObject
    {
        public string Value { get; }

        public PasswordValueObject(string password)
        {
            Value = password;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    private class AddressValueObject : ValueObject
    {
        public string City { get; }
        public string State { get; }

        public AddressValueObject(string city, string state)
        {
            City = city;
            State = state;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return State;
        }
    }

    /// <summary>
    /// Creates a new email value object.
    /// </summary>
    /// <param name="email">The email value of type <see cref="EmailValueObject"/>.</param>
    /// <returns>A new email value object of type <see cref="EmailValueObject"/>.</returns>
    public static ValueObject CreateEmailValueObject(string email)
    {
        return new EmailValueObject(email);
    }

    /// <summary>
    /// Creates a new password value object of type <see cref="PasswordValueObject"/>.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns>A new password value object of type <see cref="PasswordValueObject"/>.</returns>
    public static ValueObject CreatePasswordValueObject(string password)
    {
        return new PasswordValueObject(password);
    }

    /// <summary>
    /// Creates a new address value object of type <see cref="AddressValueObject"/>.
    /// </summary>
    /// <param name="city">The address city.</param>
    /// <param name="state">The address state.</param>
    /// <returns>A new address value object of type <see cref="AddressValueObject"/>.</returns>
    public static ValueObject CreateAddressValueObject(string city, string state)
    {
        return new AddressValueObject(city, state);
    }
}
