using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.Errors;
using Application.Orders.Queries.GetOrderById;
using Application.UnitTests.Orders.Queries.TestUtils;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Authorization;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Queries.GetOrderById;

/// <summary>
/// Unit tests for the <see cref="GetOrderByIdQueryHandler"/> handler.
/// </summary>
public class GetOrderByIdQueryHandlerTests
{
    private readonly GetOrderByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IRepository<Payment, PaymentId>> _mockPaymentRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetOrderByIdQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);

        _handler = new GetOrderByIdQueryHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Verifies that the handler returns order and payment details when the order exists and the user has access.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenOrderExistsAndUserHasAccess_ReturnsOrderDetailedResult()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();

        var orderOwnerId = UserId.Create(query.CurrentUserId);
        var orderId = OrderId.Create(query.OrderId);

        var order = await OrderUtils.CreateOrder(id: orderId, ownerId: orderOwnerId, withDefaultMockSetup: true);
        var orderOwner = UserUtils.CreateUser(id: orderOwnerId);
        var orderPayment = PaymentUtils.CreatePayment(orderId: orderId);

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(orderOwner);

        _mockPaymentRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>()))
            .ReturnsAsync(orderPayment);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Order.Should().BeEquivalentTo(order);
        result.Payment.Should().BeEquivalentTo(orderPayment);
    }

    /// <summary>
    /// Verifies that the handler throws an exception when the order does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenOrderDoesNotExist_ThrowsError()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();
        var orderId = OrderId.Create(query.OrderId);

        _mockOrderRepository
            .Setup(repo => repo.FindByIdAsync(orderId))
            .ReturnsAsync((Order)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<OrderNotFoundException>();
    }

    /// <summary>
    /// Verifies that the handler throws an exception when the user does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenUserDoesNotExist_ThrowsNotAllowedError()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();
        var orderId = OrderId.Create(query.OrderId);
        var currentUserId = UserId.Create(query.CurrentUserId);

        var order = await OrderUtils.CreateOrder(id: orderId, ownerId: currentUserId, withDefaultMockSetup: true);

        _mockOrderRepository
            .Setup(repo => repo.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync((User)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<UserNotAllowedException>();
    }

    /// <summary>
    /// Verifies that the handler throws an exception when the user does not have access to the order.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenUserDoesNotHaveAccess_ThrowsNotAllowedError()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();
        var orderId = OrderId.Create(query.OrderId);
        var currentUserId = UserId.Create(query.CurrentUserId);

        var user = UserUtils.CreateUser(currentUserId, roles: new HashSet<Role>() { Role.Customer });
        var order = await OrderUtils.CreateOrder(orderId, UserId.Create(999), withDefaultMockSetup: true);

        _mockOrderRepository
            .Setup(repo => repo.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(user);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<UserNotAllowedException>();
    }

    /// <summary>
    /// Verifies that the handler returns order details without payment when no payment is found for the order.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenOrderExistsAndNoPayment_ReturnsOrderWithoutPayment()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();
        var orderId = OrderId.Create(query.OrderId);
        var currentUserId = UserId.Create(query.CurrentUserId);

        var order = await OrderUtils.CreateOrder(id: orderId, ownerId: currentUserId, withDefaultMockSetup: true);
        var user = UserUtils.CreateUser(id: currentUserId);

        _mockOrderRepository
            .Setup(repo => repo.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(user);

        _mockPaymentRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>()))
            .ReturnsAsync((Payment)null!);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Order.Should().BeEquivalentTo(order);
        result.Payment.Should().BeNull();
    }
}
