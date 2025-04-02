using MediatR;

namespace SharedKernel.Interfaces;

/// <summary>
/// Represents a contract for domain events.
/// </summary>
public interface IDomainEvent : INotification
{
}
