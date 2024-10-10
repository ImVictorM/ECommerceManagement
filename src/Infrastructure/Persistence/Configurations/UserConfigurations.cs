using Domain.Common.ValueObjects;
using Domain.RoleAggregate;
using Domain.RoleAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="User"/> aggregate to entity framework.
/// </summary>
public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUserTable(builder);
        ConfigureUserAddressTable(builder);
        ConfigureUserRolesTable(builder);
    }

    /// <summary>
    /// Configure the user table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureUserTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder
            .Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            )
            .IsRequired();

        builder
            .Property(user => user.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value)
            )
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(user => user.Phone)
            .HasMaxLength(11);

        builder
            .Property(user => user.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => PasswordHash.Create(value)
            )
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(user => user.IsActive)
            .IsRequired();

        builder.HasIndex(user => user.Email).IsUnique();
    }

    /// <summary>
    /// Configure the user address table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureUserAddressTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(user => user.UserAddresses, userAddressBuilder =>
        {
            userAddressBuilder.ToTable("user_addresses");

            userAddressBuilder
                .HasKey(userAddress => userAddress.Id);

            userAddressBuilder
                .Property(userAddress => userAddress.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserAddressId.Create(value)
                )
                .IsRequired();

            userAddressBuilder
                .WithOwner()
                .HasForeignKey("id_user");

            userAddressBuilder
                .Property("id_user")
                .IsRequired();

            userAddressBuilder.OwnsOne(o => o.Address, addressBuilder =>
            {
                addressBuilder
                    .Property(a => a.PostalCode)
                    .HasMaxLength(10)
                    .IsRequired();
                addressBuilder
                    .Property(a => a.Street)
                    .HasMaxLength(120)
                    .IsRequired();
                addressBuilder
                    .Property(a => a.Neighborhood)
                    .HasMaxLength(120);
                addressBuilder
                    .Property(a => a.State)
                    .HasMaxLength(120)
                    .IsRequired();
                addressBuilder
                    .Property(a => a.City)
                    .HasMaxLength(120)
                    .IsRequired();
            });
        });
    }

    /// <summary>
    /// Configure the user role table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureUserRolesTable(EntityTypeBuilder<User> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(User.UserRoles))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(user => user.UserRoles, userRolesBuilder =>
        {
            userRolesBuilder.ToTable("users_roles");

            userRolesBuilder.HasKey(userRole => userRole.Id);

            userRolesBuilder.WithOwner().HasForeignKey("id_user");

            userRolesBuilder
                .Property("id_user")
                .IsRequired();

            userRolesBuilder
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRolesBuilder
                .Property(userRole => userRole.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserRoleId.Create(value)
                )
                .IsRequired();

            userRolesBuilder
                .Property(userRole => userRole.RoleId)
                .HasConversion(
                    roleId => roleId.Value,
                    value => RoleId.Create(value)
                )
                .IsRequired();
        });
    }
}
