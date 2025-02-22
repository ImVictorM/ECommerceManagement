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
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCanceledRestockProductsHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The product repository.</param>
    public OrderCanceledRestockProductsHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCanceled notification, CancellationToken cancellationToken)
    {
        var order = notification.Order;
        var productIds = order.Products.Select(p => p.ProductId);

        var products = await _productRepository.FindAllAsync(p => productIds.Contains(p.Id), cancellationToken);

        var productsHashMap = products.ToDictionary(p => p.Id);

        foreach (var orderProduct in order.Products)
        {
            var product = productsHashMap[orderProduct.ProductId];

            product.Inventory.AddStock(orderProduct.Quantity);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
