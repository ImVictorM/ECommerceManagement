using MediatR;

namespace Domain.Common.Interfaces;

/// <summary>
/// Contract for domain events.
/// </summary>
public interface IDomainEvent : INotification
{
}
