using Domain.Common.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.UserAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="User"/> aggregate.
/// </summary>
public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUserTable(builder);
        ConfigureOwnedUserAddressTable(builder);
        ConfigureOwnedUserRolesTable(builder);
    }

    /// <summary>
    /// Configure the users table.
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
            .ValueGeneratedOnAdd()
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
    /// Configure the user_addresses table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedUserAddressTable(EntityTypeBuilder<User> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(User.UserAddresses))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

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
                .ValueGeneratedOnAdd()
                .IsRequired();

            userAddressBuilder
                .WithOwner()
                .HasForeignKey("id_user");

            userAddressBuilder
                .Property("id_user")
                .IsRequired();

            userAddressBuilder.OwnsOne(o => o.Address, AddressNavigationBuilderConfigurations.Configure);
        });
    }

    /// <summary>
    /// Configure the users_roles table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedUserRolesTable(EntityTypeBuilder<User> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(User.UserRoles))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(user => user.UserRoles, userRolesBuilder =>
        {
            userRolesBuilder.ToTable("users_roles");

            userRolesBuilder.HasKey(userRole => userRole.Id);

            userRolesBuilder
                .Property(userRole => userRole.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserRoleId.Create(value)
                )
                .ValueGeneratedOnAdd()
                .IsRequired();


            userRolesBuilder
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRolesBuilder.WithOwner().HasForeignKey("id_user");

            userRolesBuilder
                .Property("id_user")
                .IsRequired();
        });
    }
}
