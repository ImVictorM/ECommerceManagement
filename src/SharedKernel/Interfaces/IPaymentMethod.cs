namespace SharedKernel.Interfaces;

/// <summary>
/// Represents a payment method.
/// </summary>
public interface IPaymentMethod
{
    /// <summary>
    /// Gets the payment method type.
    /// </summary>
    public string Type { get; }
}
