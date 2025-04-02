using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Sales.Commands.DeleteSale;

/// <summary>
/// Represents a command to delete an existent sale.
/// </summary>
/// <param name="SaleId">The sale identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeleteSaleCommand(string SaleId) : IRequestWithAuthorization<Unit>;
