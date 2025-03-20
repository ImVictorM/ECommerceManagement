using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.DTOs;
using Application.Sales.Commands.CreateSale;
using Application.UnitTests.Sales.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales.Commands.CreateSale;

/// <summary>
/// Unit tests for the <see cref="CreateSaleCommandHandler"/> command handler.
/// </summary>
public class CreateSaleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly Mock<ISaleEligibilityService> _mockSaleEligibilityService;
    private readonly CreateSaleCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleCommandHandlerTests"/>
    /// class.
    /// </summary>
    public CreateSaleCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockSaleEligibilityService = new Mock<ISaleEligibilityService>();

        _handler = new CreateSaleCommandHandler(
            _mockSaleRepository.Object,
            _mockSaleEligibilityService.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<CreateSaleCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies a sale is successfully created and saved.
    /// </summary>
    [Fact]
    public async Task HandleCreateSale_WithValidRequest_ShouldCreateAndSaveSale()
    {
        var idCreatedSale = SaleId.Create(1);

        _mockUnitOfWork
            .MockSetEntityIdBehavior<ISaleRepository, Sale, SaleId>(
                _mockSaleRepository,
                idCreatedSale
            );

        _mockSaleEligibilityService
            .Setup(s => s.IsSaleEligibleAsync(
                It.IsAny<Sale>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        var request = CreateSaleCommandUtils.CreateCommand(
            productOnSaleIds: ["1", "2"]
        );

        var result = await _handler.Handle(request, default);

        _mockSaleRepository.Verify(
            r => r.AddAsync(It.IsAny<Sale>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once());

        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
        result.Id.Should().Be(idCreatedSale.ToString());
    }
}
