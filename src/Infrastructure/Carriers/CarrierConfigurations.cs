using Application.Common.Security.Authentication;

using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using SharedKernel.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Carriers;

/// <summary>
/// Configures the tables for the <see cref="Carrier"/> aggregate.
/// </summary>
public class CarrierConfigurations : EntityTypeConfigurationDependency<Carrier>
{
    private sealed record CarrierData(
        CarrierId Id,
        string Name,
        Email Email,
        PasswordHash PasswordHash,
        string? Phone,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt,
        long _roleId
    );

    private readonly CarrierData _defaultInternalCarrier;

    /// <summary>
    /// Initiates a new instance of the <see cref="CarrierConfigurations"/> class.
    /// </summary>
    /// <param name="carrierOptions">The default internal carrier options.</param>
    /// <param name="passwordHasher">The password hasher to hash the carrier password.</param>
    public CarrierConfigurations(IOptions<CarrierInternalSettings> carrierOptions, IPasswordHasher passwordHasher)
    {
        var carrierSettings = carrierOptions.Value;

        _defaultInternalCarrier = new CarrierData(
            CarrierId.Create(1),
            carrierSettings.Name,
            Email.Create(carrierSettings.Email),
            passwordHasher.Hash(carrierSettings.Password),
            carrierSettings.Phone,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            Role.Carrier.Id
        );
    }

    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<Carrier> builder)
    {
        builder.ToTable("carriers");

        builder.HasKey(c => c.Id);

        builder
            .Property<long>("_roleId")
            .HasColumnName("id_role")
            .IsRequired();

        builder
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey("_roleId")
            .IsRequired();

        builder.Ignore(c => c.Role);

        builder
            .Property(c => c.Id)
            .HasConversion(id => id.Value, value => CarrierId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(c => c.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(c => c.Email)
            .HasConversion(email => email.Value, value => Email.Create(value))
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(c => c.PasswordHash)
            .HasConversion(hash => hash.Value, value => PasswordHash.Create(value))
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(c => c.Phone)
            .HasMaxLength(11)
            .IsRequired(false);

        builder.HasIndex(c => c.Email).IsUnique();

        builder.HasData(_defaultInternalCarrier);
    }
}
