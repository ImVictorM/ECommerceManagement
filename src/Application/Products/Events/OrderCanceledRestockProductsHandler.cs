using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.OrderAggregate.Events;
using Domain.ProductAggregate.Specifications;

using MediatR;

namespace Application.Products.Events;

internal sealed class OrderCanceledRestockProductsHandler
    : INotificationHandler<OrderCanceled>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public OrderCanceledRestockProductsHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task Handle(
        OrderCanceled notification,
        CancellationToken cancellationToken
    )
    {
        var order = notification.Order;
        var productIds = order.Products.Select(p => p.ProductId);

        var products = await _productRepository.FindSatisfyingAsync(
            new QueryProductsContainingIdsSpecification(productIds),
            cancellationToken
        );

        var productsHashMap = products.ToDictionary(p => p.Id);

        foreach (var orderProduct in order.Products)
        {
            var product = productsHashMap[orderProduct.ProductId];

            product.Inventory.AddStock(orderProduct.Quantity);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
