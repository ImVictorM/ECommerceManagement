using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents a pix payment method.
/// </summary>
public class PixPaymentMethod : ValueObject, IPaymentMethod
{
    /// <inheritdoc/>
    public string Type => "pix";

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
    }
}
