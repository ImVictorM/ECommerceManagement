using Application.UnitTests.TestUtils.Events.Orders;
using Application.Users.Events;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;
using Moq;
using Application.Common.Persistence;

namespace Application.UnitTests.Users.Events;

/// <summary>
/// Unit tests for the <see cref="OrderCreatedAssignUserAddressesHandler"/> event handler.
/// </summary>
public class OrderCreatedAssignUserAddressesHandlerTests
{
    private readonly OrderCreatedAssignUserAddressesHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedAssignUserAddressesHandlerTests"/> class.
    /// </summary>
    public OrderCreatedAssignUserAddressesHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new OrderCreatedAssignUserAddressesHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests the handler assigns the billing and delivery addresses to the user.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WithExistingUser_AssignsTheBillingAndDeliveryAddressesToTheUser()
    {
        var user = UserUtils.CreateUser(id: UserId.Create(1));

        var deliveryAddress = AddressUtils.CreateAddress();
        var billingAddress = AddressUtils.CreateAddress();

        var order = await OrderUtils.CreateOrderAsync(
            ownerId: user.Id,
            deliveryAddress: deliveryAddress
        );

        var notification = await OrderCreatedUtils.CreateEventAsync(order: order, billingAddress: billingAddress);

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync(user);

        await _handler.Handle(notification, default);

        user.UserAddresses.Should().Contain(deliveryAddress);
        user.UserAddresses.Should().Contain(billingAddress);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
