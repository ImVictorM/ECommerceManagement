using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Value object utilities.
/// </summary>
public static class ValueObjectUtils
{
    /// <summary>
    /// Represents an email implementation of a value object.
    /// </summary>
    public class EmailValueObject : ValueObject
    {
        /// <summary>
        /// Gets the email value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initiates a new instanc of the <see cref="EmailValueObject"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        private EmailValueObject(string email)
        {
            Value = email;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EmailValueObject"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A new instance of the <see cref="EmailValueObject"/> class.</returns>
        public static EmailValueObject Create(string email)
        {
            return new EmailValueObject(email);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    /// <summary>
    /// Represetnts a password implementaion of a value object.
    /// </summary>
    public class PasswordValueObject : ValueObject
    {
        /// <summary>
        /// Gets the password value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initiates a new instanc of the <see cref="PasswordValueObject"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        private PasswordValueObject(string password)
        {
            Value = password;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PasswordValueObject"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>A new instance of the <see cref="PasswordValueObject"/> class.</returns>
        public static PasswordValueObject Create(string password)
        {
            return new PasswordValueObject(password);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    /// <summary>
    /// Represets an address implementation of a value object.
    /// </summary>
    public class AddressValueObject : ValueObject
    {
        /// <summary>
        /// Gets the address city.
        /// </summary>
        public string City { get; }
        /// <summary>
        /// Gets the address state.
        /// </summary>
        public string State { get; }

        /// <summary>
        /// Initiates a new instanc of the <see cref="AddressValueObject"/> class.
        /// </summary>
        /// <param name="city">The address city.</param>
        /// <param name="state">The address state.</param>
        private AddressValueObject(string city, string state)
        {
            City = city;
            State = state;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AddressValueObject"/> class.
        /// </summary>
        /// <param name="city">The address city.</param>
        /// <param name="state">The address state.</param>
        /// <returns>A new instance of the <see cref="AddressValueObject"/> class.</returns>
        public static AddressValueObject Create(string city, string state)
        {
            return new AddressValueObject(city, state);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return State;
        }
    }
}
