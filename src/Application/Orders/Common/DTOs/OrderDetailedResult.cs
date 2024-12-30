using Application.Common.Interfaces.Payments;
using Domain.OrderAggregate;

namespace Application.Orders.Common.DTOs;

/// <summary>
/// Represents an order result.
/// </summary>
/// <param name="Order">The order.</param>
/// <param name="Payment">The order payment.</param>
public record OrderDetailedResult(
    Order Order,
    IPaymentResponse? Payment
);
