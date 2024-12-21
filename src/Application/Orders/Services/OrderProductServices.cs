using Application.Common.Interfaces.Persistence;
using Application.Orders.Commands.Common.Errors;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;

namespace Application.Orders.Services;

/// <summary>
/// Represents services related to order products.
/// </summary>
public class OrderProductServices : IOrderProductService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderProductServices"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderProductServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateTotalAsync(IEnumerable<OrderProduct> orderProducts)
    {
        // TODO: fix this
        //var productIds = orderProducts.Select(p => p.ProductId);
        //var products = await _unitOfWork.ProductRepository.FindAllAsync(p => productIds.Contains(p.Id));

        //var total = 0m;
        //foreach (var orderProduct in orderProducts)
        //{
        //    var product = products.First(p => p.Id == orderProduct.ProductId);
        //    total += product.GetPriceAfterDiscounts() * orderProduct.Quantity;
        //}

        //return total;

        return 100m;
    }

    /// <inheritdoc/>
    public async Task VerifyInventoryAvailabilityAsync(IEnumerable<OrderProduct> orderProducts)
    {
        var productIds = orderProducts.Select(p => p.ProductId);
        var products = await _unitOfWork.ProductRepository.FindAllAsync(p => productIds.Contains(p.Id));

        foreach (var orderProduct in orderProducts)
        {
            var product = products.First(p => p.Id == orderProduct.ProductId);

            if (!product.Inventory.HasInventoryAvailable(orderProduct.Quantity))
            {
                throw new ProductNotAvailableException($"{product.Name} is not available. Current inventory: {product.Inventory.QuantityAvailable}. Order quantity: {orderProduct.Quantity}");
            }
        }
    }
}
