using Application.Common.PaymentGateway.Responses;

namespace Application.Orders.DTOs.Results;

/// <summary>
/// Represents an order payment result.
/// </summary>
public class OrderPaymentResult
{
    /// <summary>
    /// Gets the payment identifier.
    /// </summary>
    public string PaymentId { get; }
    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public decimal Amount { get; }
    /// <summary>
    /// Gets the payment quantity of installments.
    /// </summary>
    public int Installments { get; }
    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public string Status { get; }
    /// <summary>
    /// Gets the payment details.
    /// </summary>
    public string Details { get; }
    /// <summary>
    /// Gets the payment method.
    /// </summary>
    public string PaymentMethod { get; }

    private OrderPaymentResult(PaymentResponse response)
    {
        PaymentId = response.PaymentId.ToString();
        Amount = response.Amount;
        Installments = response.Installments;
        Status = response.Status.Name;
        Details = response.Details;
        PaymentMethod = response.PaymentMethod.Type;
    }

    internal static OrderPaymentResult FromResponse(PaymentResponse response)
    {
        return new OrderPaymentResult(response);
    }
}
