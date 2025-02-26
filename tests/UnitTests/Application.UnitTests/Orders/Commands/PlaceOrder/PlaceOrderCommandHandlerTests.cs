using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Orders.Commands.PlaceOrder;
using Application.UnitTests.Orders.Commands.TestUtils;
using Application.UnitTests.Orders.TestUtils.Extensions;
using Application.UnitTests.TestUtils.Extensions;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Commands.PlaceOrder;

/// <summary>
/// Unit tests for the <see cref="PlaceOrderCommandHandler"/> command handler.
/// </summary>
public class PlaceOrderCommandHandlerTests
{
    private readonly PlaceOrderCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOrderService> _mockOrdersService;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IIdentityProvider> _mockIdentityProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandlerTests"/> class.
    /// </summary>
    public PlaceOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockOrdersService = new Mock<IOrderService>();
        _mockIdentityProvider = new Mock<IIdentityProvider>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new PlaceOrderCommandHandler(
            _mockUnitOfWork.Object,
            _mockOrderRepository.Object,
            _mockOrdersService.Object,
            _mockIdentityProvider.Object,
            new Mock<ILogger<PlaceOrderCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests the command handler handles a valid command successfully, creating an order and saving it.
    /// </summary>
    [Fact]
    public async Task HandlePlaceOrder_WhenRequestIsValid_CreatesOrder()
    {
        var mockIdentityUser = new IdentityUser("1", [Role.Customer]);
        var reservedProducts = OrderUtils.CreateReservedProducts(3).ToList();
        var orderProducts = reservedProducts
            .Select(rp => OrderUtils.CreateOrderProduct(productId: rp.ProductId, quantity: rp.Quantity))
            .ToList();
        var mockTotal = orderProducts.Sum(op => op.CalculateTransactionPrice());

        var command = PlaceOrderCommandUtils.CreateCommand(
            orderProducts: reservedProducts.ParseToInput(),
            couponsAppliedIds: ["1", "2"],
            installments: 2
        );

        var mockCreatedId = OrderId.Create(2);

        _mockIdentityProvider
            .Setup(i => i.GetCurrentUserIdentity())
            .Returns(mockIdentityUser);

        _mockOrdersService
            .Setup(s => s.PrepareOrderProductsAsync(
                command.Products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(orderProducts);

        _mockOrdersService
            .Setup(s => s.CalculateTotalAsync(
                orderProducts,
                It.IsAny<ShippingMethodId>(),
                It.IsAny<IEnumerable<OrderCoupon>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(mockTotal);

        _mockUnitOfWork.MockSetEntityIdBehavior<IOrderRepository, Order, OrderId>(
            _mockOrderRepository,
            mockCreatedId
        );

        var result = await _handler.Handle(command, default);

        result.Id.Should().Be(mockCreatedId.ToString());

        _mockOrderRepository.Verify(
            r => r.AddAsync(It.Is<Order>(o =>
                o.Total == mockTotal
                && o.Products.All(orderProducts.Contains)
                && o.OwnerId.ToString() == mockIdentityUser.Id
            )),
            Times.Once()
        );
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
