using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;
using Application.UnitTests.Common.Security.Authorization.TestUtils;

using Domain.CarrierAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Common.Security.Authorization.Policies;

/// <summary>
/// Unit tests for the <see cref="ShipmentCarrierPolicy{T}"/> policy.
/// </summary>
public class ShipmentCarrierPolicyTests
{
    private readonly ShipmentCarrierPolicy<IShipmentSpecificResource> _policy;
    private readonly Mock<IShipmentRepository> _mockShipmentRepository;
    private readonly Mock<IIdentityProvider> _mockIdentityProvider;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ShipmentCarrierPolicyTests"/> class.
    /// </summary>
    public ShipmentCarrierPolicyTests()
    {
        _mockShipmentRepository = new Mock<IShipmentRepository>();
        _mockIdentityProvider = new Mock<IIdentityProvider>();

        _policy = new ShipmentCarrierPolicy<IShipmentSpecificResource>(
            _mockIdentityProvider.Object,
            _mockShipmentRepository.Object
        );
    }

    /// <summary>
    /// Verifies the policy returns true when the shipment is null.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenShipmentIsNull_ReturnsTrue()
    {
        var shipmentId = ShipmentId.Create(1);
        var request = new RequestUtils.TestRequestWithoutAuthShipmentRelated(
            shipmentId.ToString()
        );

        var identity = new IdentityUser("1", [Role.Carrier]);

        _mockIdentityProvider
            .Setup(ip => ip.GetCurrentUserIdentity())
            .Returns(identity);

        _mockShipmentRepository
            .Setup(repo => repo.FindByIdAsync(
                shipmentId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Shipment?)null);

        var result = await _policy.IsAuthorizedAsync(
            request,
            new IdentityUser("1", [Role.Carrier])
        );

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns true when the current user is the shipment
    /// carrier.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenCarrierOwnsShipment_ReturnsTrue()
    {
        var shipmentId = ShipmentId.Create(1);
        var identity = new IdentityUser("1", [Role.Carrier]);
        var shipment = ShipmentUtils.CreateShipment(
            id: shipmentId,
            carrierId: CarrierId.Create(identity.Id)
        );
        var request = new RequestUtils.TestRequestWithoutAuthShipmentRelated(
            shipmentId.ToString()
        );

        _mockIdentityProvider
            .Setup(ip => ip.GetCurrentUserIdentity())
            .Returns(identity);

        _mockShipmentRepository
            .Setup(repo => repo.FindByIdAsync(
                shipmentId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shipment);

        var result = await _policy.IsAuthorizedAsync(request, identity);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns false when the current user is not the
    /// shipment carrier.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenCarrierDoesNotOwnShipment_ReturnsFalse()
    {
        var shipmentId = ShipmentId.Create(1);
        var identity = new IdentityUser("1", [Role.Carrier]);
        var shipment = ShipmentUtils.CreateShipment(
            id: shipmentId,
            carrierId: CarrierId.Create("2")
        );
        var request = new RequestUtils.TestRequestWithoutAuthShipmentRelated(
            shipmentId.ToString()
        );

        _mockIdentityProvider
            .Setup(ip => ip.GetCurrentUserIdentity())
            .Returns(identity);

        _mockShipmentRepository
            .Setup(repo => repo.FindByIdAsync(
                shipmentId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shipment);

        var result = await _policy.IsAuthorizedAsync(request, identity);

        result.Should().BeFalse();
    }
}
