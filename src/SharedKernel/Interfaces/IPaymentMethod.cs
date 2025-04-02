namespace SharedKernel.Interfaces;

/// <summary>
/// Represents a payment method.
/// </summary>
public interface IPaymentMethod
{
    /// <summary>
    /// Gets the payment method name.
    /// </summary>
    public string Name { get; }
}
