using Application.UnitTests.TestUtils.Events.Orders;
using Application.Users.Events;
using Application.Common.Persistence;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Events;

/// <summary>
/// Unit tests for the <see cref="OrderCreatedAssignUserAddressesHandler"/> event handler.
/// </summary>
public class OrderCreatedAssignUserAddressesHandlerTests
{
    private readonly OrderCreatedAssignUserAddressesHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedAssignUserAddressesHandlerTests"/> class.
    /// </summary>
    public OrderCreatedAssignUserAddressesHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new OrderCreatedAssignUserAddressesHandler(
            _mockUnitOfWork.Object,
            _mockUserRepository.Object
        );
    }

    /// <summary>
    /// Tests the handler assigns the billing and delivery addresses to the user.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WithExistingUser_AssignsTheBillingAndDeliveryAddressesToTheUser()
    {
        var user = UserUtils.CreateCustomer(id: UserId.Create(1));

        var deliveryAddress = AddressUtils.CreateAddress();
        var billingAddress = AddressUtils.CreateAddress();

        var order = await OrderUtils.CreateOrderAsync(
            ownerId: user.Id,
            deliveryAddress: deliveryAddress,
            billingAddress: billingAddress
        );

        var notification = await OrderCreatedUtils.CreateEventAsync(
            order: order,
            billingAddress: billingAddress,
            deliveryAddress: deliveryAddress
        );

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<User>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(user);

        await _handler.Handle(notification, default);

        user.UserAddresses.Should().Contain(deliveryAddress);
        user.UserAddresses.Should().Contain(billingAddress);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
