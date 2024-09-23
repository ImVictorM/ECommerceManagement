using Domain.AddressAggregate;
using Domain.AddressAggregate.ValueObjects;
using Domain.RoleAggregate;
using Domain.RoleAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUser(builder);
        ConfigureUserAddress(builder);
        ConfigureUserRoles(builder);
    }

    private static void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id).HasName(nameof(User.Id).ToLowerInvariant());

        builder
            .Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            );

        builder
            .Property(user => user.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(user => user.Phone)
            .HasMaxLength(11);

        builder
            .Property(user => user.PasswordHash)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(user => user.Email).IsUnique();
    }

    private static void ConfigureUserAddress(EntityTypeBuilder<User> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(User.Addresses))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(user => user.Addresses, userAddressBuilder =>
        {
            userAddressBuilder.ToTable("users_addresses");

            userAddressBuilder
                .HasKey(userAddress => userAddress.Id)
                .HasName(nameof(UserAddress.Id).ToLowerInvariant());

            userAddressBuilder.WithOwner().HasForeignKey("id_user");

            userAddressBuilder
                .HasOne<Address>()
                .WithMany()
                .HasForeignKey(address => address.AddressId);

            userAddressBuilder
                .Property(userAddress => userAddress.AddressId)
                .HasConversion(
                    id => id.Value,
                    value => AddressId.Create(value)
                );

            userAddressBuilder
                .Property(userAddress => userAddress.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserAddressId.Create(value)
                );
        });
    }

    private static void ConfigureUserRoles(EntityTypeBuilder<User> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(User.Roles))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(user => user.Roles, userRolesBuilder =>
        {
            userRolesBuilder.ToTable("users_roles");

            userRolesBuilder
                .HasKey(userRole => userRole.Id)
                .HasName(nameof(UserRole.Id).ToLowerInvariant());

            userRolesBuilder.WithOwner().HasForeignKey("id_user");

            userRolesBuilder
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(role => role.RoleId);


            userRolesBuilder
                .Property(userRole => userRole.RoleId)
                .HasConversion(
                    id => id.Value,
                    value => RoleId.Create(value)
                );

            userRolesBuilder
                .Property(userRole => userRole.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserRoleId.Create(value)
                );
        });
    }
}
