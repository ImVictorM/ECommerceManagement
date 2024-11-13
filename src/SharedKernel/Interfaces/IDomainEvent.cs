using MediatR;

namespace SharedKernel.Interfaces;

/// <summary>
/// Contract for domain events.
/// </summary>
public interface IDomainEvent : INotification
{
}
