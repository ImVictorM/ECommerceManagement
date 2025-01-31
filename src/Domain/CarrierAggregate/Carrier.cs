using Domain.CarrierAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.CarrierAggregate;

/// <summary>
/// Represents a shipment carrier.
/// </summary>
public sealed class Carrier : AggregateRoot<CarrierId>
{
    /// <summary>
    /// Gets the carrier name.
    /// </summary>
    public string Name { get; private set; } = null!;

    private Carrier() { }

    private Carrier(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Carrier"/> class.
    /// </summary>
    /// <param name="name">The carrier name.</param>
    /// <returns>A new instance of the <see cref="Carrier"/> class.</returns>
    public static Carrier Create(string name)
    {
        return new Carrier(name);
    }
}
