using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents a payment slip payment method.
/// </summary>
public class PaymentSlipPaymentMethod : ValueObject, IPaymentMethod
{
    /// <inheritdoc/>
    public string Type => "payment_slip";

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
    }
}
