using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Sales.Commands.UpdateSale;
using Application.Sales.Errors;
using Application.UnitTests.Sales.Commands.TestUtils;

using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales.Commands.UpdateSale;

/// <summary>
/// Unit tests for the <see cref="UpdateSaleCommandHandler"/> command handler.
/// </summary>
public class UpdateSaleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly Mock<ISaleEligibilityService> _mockSaleEligibilityService;
    private readonly UpdateSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UpdateSaleCommandHandlerTests"/> class.
    /// </summary>
    public UpdateSaleCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockSaleEligibilityService = new Mock<ISaleEligibilityService>();

        _handler = new UpdateSaleCommandHandler(
            _mockSaleRepository.Object,
            _mockSaleEligibilityService.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<UpdateSaleCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies a sale is successfully updated when it exists.
    /// </summary>
    [Fact]
    public async Task HandleUpdateSaleCommand_WithExistentSale_UpdatesSale()
    {
        var saleId = "1";
        var sale = SaleUtils.CreateSale(id: SaleId.Create(saleId));
        var request = UpdateSaleCommandUtils.CreateCommand(
            saleId: saleId,
            categoryOnSaleIds: ["1", "2"]
        );

        _mockSaleRepository
            .Setup(r => r.FindByIdAsync(
                sale.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(sale);

        await _handler.Handle(request, default);

        _mockSaleRepository.Verify(
            r => r.FindByIdAsync(
                sale.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _mockSaleEligibilityService.Verify(
            s => s.EnsureSaleProductsEligibilityAsync(
                sale,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies an exception is thrown when trying to update a non-existent sale.
    /// </summary>
    [Fact]
    public async Task HandleUpdateSaleCommand_WithNonExistentSale_ThrowsError()
    {
        var saleId = SaleId.Create(1);
        var request = UpdateSaleCommandUtils.CreateCommand(saleId: saleId.ToString());

        _mockSaleRepository
            .Setup(r => r.FindByIdAsync(
                saleId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Sale?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<SaleNotFoundException>();
    }
}
