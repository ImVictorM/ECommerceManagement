using Application.Common.Interfaces.Persistence;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Handles the process of placing an order.
/// </summary>
public sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderServices _orderPricingService;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="orderPricingService">The order service.</param>
    public PlaceOrderCommandHandler(IUnitOfWork unitOfWork, IOrderServices orderPricingService)
    {
        _unitOfWork = unitOfWork;
        _orderPricingService = orderPricingService;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify inventory availability
        var userId = UserId.Create(request.UserId);

        var orderProducts = request.Products
            .Select(p => OrderProduct.Create(ProductId.Create(p.Id), p.Quantity))
            .ToList();

        await _orderPricingService.VerifyInventoryAvailabilityAsync(orderProducts);

        // 3. Calculate the order total

        var total = await _orderPricingService.CalculateTotalAsync(orderProducts);

        // 4. Create the order

        var order = Order.Create(
            userId,
            orderProducts,
            total,
            request.PaymentMethod,
            request.Installments
        );

        // 5. Save order / Handle order created event

        await _unitOfWork.OrderRepository.AddAsync(order);

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
