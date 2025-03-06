using Application.Common.DTOs;
using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Coupons.Commands.CreateCoupon;
using Application.Coupons.DTOs;
using Application.UnitTests.Coupons.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Commands.CreateCoupon;

/// <summary>
/// Unit tests for the <see cref="CreateCouponCommandHandler"/> class.
/// </summary>
public class CreateCouponCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICouponRepository> _mockCouponRepository;
    private readonly CreateCouponCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CreateCouponCommandHandlerTests"/> class.
    /// </summary>
    public CreateCouponCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCouponRepository = new Mock<ICouponRepository>();

        _handler = new CreateCouponCommandHandler(
            _mockUnitOfWork.Object,
            _mockCouponRepository.Object,
            Mock.Of<ILogger<CreateCouponCommandHandler>>()
        );
    }

    /// <summary>
    /// Collection containing valid <see cref="CreateCouponCommand"/> commands.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidCreateCouponCommands =
    [
        [
            CreateCouponCommandUtils.CreateCommand()
        ],
        [
            CreateCouponCommandUtils.CreateCommand(
                restrictions:
                [
                    new ProductRestrictionInput(["1", "3", "401"]),
                    new CategoryRestrictionInput(
                        ["1", "3"],
                        ["401"]
                    )
                ]
            )
        ]
    ];

    /// <summary>
    /// Verifies a coupon is successfully created and saved.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidCreateCouponCommands))]
    public async Task HandleCreateCoupon_WithValidRequest_CreatesAndSavesCoupon(
        CreateCouponCommand request
    )
    {
        var idCreatedCoupon = CouponId.Create(42);

        _mockUnitOfWork
            .MockSetEntityIdBehavior<ICouponRepository, Coupon, CouponId>(
                _mockCouponRepository,
                idCreatedCoupon
            );

        var result = await _handler.Handle(request, default);

        _mockCouponRepository.Verify(
            r => r.AddAsync(It.IsAny<Coupon>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once());

        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
        result.Id.Should().Be(idCreatedCoupon.ToString());
    }
}
