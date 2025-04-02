using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Orders.Commands.PlaceOrder;
using Application.UnitTests.Orders.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
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
    private readonly Mock<IOrderAssemblyService> _mockOrderAssemblyService;
    private readonly Mock<IOrderPricingService> _mockOrderPricingService;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IIdentityProvider> _mockIdentityProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandlerTests"/>
    /// class.
    /// </summary>
    public PlaceOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockOrderAssemblyService = new Mock<IOrderAssemblyService>();
        _mockOrderPricingService = new Mock<IOrderPricingService>();
        _mockIdentityProvider = new Mock<IIdentityProvider>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new PlaceOrderCommandHandler(
            _mockUnitOfWork.Object,
            _mockOrderRepository.Object,
            _mockOrderAssemblyService.Object,
            _mockOrderPricingService.Object,
            _mockIdentityProvider.Object,
            Mock.Of<ILogger<PlaceOrderCommandHandler>>()
        );
    }

    /// <summary>
    /// Tests the command handler handles a valid command successfully,
    /// creating an order and saving it.
    /// </summary>
    [Fact]
    public async Task HandlePlaceOrderCommand_WithValidRequest_CreatesOrder()
    {
        var mockIdentityUser = new IdentityUser("1", [Role.Customer]);

        var lineItemInputs = PlaceOrderCommandUtils.CreateOrderLineItemInputs(3);
        var lineItemDrafts = lineItemInputs
            .Select(input => OrderLineItemDraft.Create(
                ProductId.Create(input.ProductId),
                input.Quantity
            ));
        var lineItems = lineItemDrafts
            .Select(d => OrderUtils.CreateOrderLineItem(
                productId: d.ProductId,
                quantity: d.Quantity
            ))
            .ToList();

        var mockTotal = lineItems.Sum(op => op.CalculateTransactionPrice());

        var command = PlaceOrderCommandUtils.CreateCommand(
            products: lineItemInputs,
            couponsAppliedIds: ["1", "2"],
            installments: 2
        );

        var mockCreatedId = OrderId.Create(2);

        _mockIdentityProvider
            .Setup(i => i.GetCurrentUserIdentity())
            .Returns(mockIdentityUser);

        _mockOrderAssemblyService
            .Setup(s => s.AssembleOrderLineItemsAsync(
                lineItemDrafts,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(lineItems);

        _mockOrderPricingService
            .Setup(s => s.CalculateTotalAsync(
                lineItems,
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
                && o.Products.All(lineItems.Contains)
                && o.OwnerId.ToString() == mockIdentityUser.Id
            )),
            Times.Once()
        );
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
