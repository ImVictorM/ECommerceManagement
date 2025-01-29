using Application.Common.Persistence;

using Domain.OrderAggregate.Events;

using MediatR;

namespace Application.Products.Events;

/// <summary>
/// Handles the <see cref="OrderCanceled"/> event by
/// restocking the order reserved products.
/// </summary>
public class OrderCanceledRestockProductsHandler : INotificationHandler<OrderCanceled>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCanceledRestockProductsHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCanceledRestockProductsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCanceled notification, CancellationToken cancellationToken)
    {
        var order = notification.Order;
        var productIds = order.Products.Select(p => p.ProductId);
        var products = await _unitOfWork.ProductRepository.FindAllAsync(p => productIds.Contains(p.Id));
        var productsHashMap = products.ToDictionary(p => p.Id);

        foreach (var orderProduct in order.Products)
        {
            var product = productsHashMap[orderProduct.ProductId];

            product.Inventory.AddStock(orderProduct.Quantity);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
