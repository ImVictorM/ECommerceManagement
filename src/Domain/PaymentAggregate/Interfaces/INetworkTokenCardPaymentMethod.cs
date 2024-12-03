namespace Domain.PaymentAggregate.Abstracts;

/// <summary>
/// Represents a card payment using network token.
/// </summary>
public interface INetworkTokenCardPaymentMethod : ICardPaymentMethod
{
    /// <summary>
    /// Gets the network token cryptograms.
    /// </summary>
    public string Cryptograms { get; }
}
