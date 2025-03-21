using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Sales.Commands.DeleteSale;
using Application.Sales.Errors;
using Application.UnitTests.Sales.Commands.TestUtils;

using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales.Commands.DeleteSale;

/// <summary>
/// Unit tests for the <see cref="DeleteSaleCommandHandler"/> command handler.
/// </summary>
public class DeleteSaleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly DeleteSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DeleteSaleCommandHandlerTests"/> class.
    /// </summary>
    public DeleteSaleCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockSaleRepository = new Mock<ISaleRepository>();

        _handler = new DeleteSaleCommandHandler(
            _mockSaleRepository.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<DeleteSaleCommandHandler>>()
        );
    }

    /// <summary>
    /// Ensures a sale is successfully deleted when it exists.
    /// </summary>
    [Fact]
    public async Task HandleDeleteSaleCommand_WithExistingSale_DeletesSale()
    {
        var saleId = "1";
        var sale = SaleUtils.CreateSale(id: SaleId.Create(saleId));

        _mockSaleRepository
            .Setup(r => r.FindByIdAsync(
                sale.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(sale);

        var request = DeleteSaleCommandUtils.CreateCommand(saleId: saleId);

        await _handler.Handle(request, default);

        _mockSaleRepository.Verify(
            r => r.RemoveOrDeactivate(sale),
            Times.Once()
        );

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Ensures an exception is thrown when trying to delete a non-existent sale.
    /// </summary>
    [Fact]
    public async Task HandleDeleteSaleCommand_WithNonExistentSale_ThrowsError()
    {
        var saleId = SaleId.Create(1);

        _mockSaleRepository
            .Setup(r => r.FindByIdAsync(
                saleId,
                It.IsAny<CancellationToken>()
             ))
            .ReturnsAsync((Sale?)null);

        var request = new DeleteSaleCommand(saleId.ToString());

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<SaleNotFoundException>();
    }
}
