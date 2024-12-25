using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Authorization;
using SharedKernel.ValueObjects;

namespace Infrastructure.Persistence.Configurations.UserAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="User"/> aggregate.
/// </summary>
public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureOwnedUserAddressesTable(builder);
        ConfigureOwnedUsersRolesTable(builder);
    }

    private static void ConfigureUsersTable(EntityTypeBuilder<User> builder)
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

    private static void ConfigureOwnedUserAddressesTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(user => user.UserAddresses, userAddressBuilder =>
        {
            userAddressBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            userAddressBuilder.ToTable("user_addresses");

            userAddressBuilder.Property<long>("id").ValueGeneratedOnAdd().IsRequired();

            userAddressBuilder.HasKey("id");

            userAddressBuilder
                .WithOwner()
                .HasForeignKey("id_user");

            userAddressBuilder
                .Property("id_user")
                .IsRequired();

            AddressNavigationBuilderConfigurations.Configure(userAddressBuilder);
        });
    }

    private static void ConfigureOwnedUsersRolesTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(user => user.UserRoles, userRolesBuilder =>
        {
            userRolesBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            userRolesBuilder.ToTable("users_roles");

            userRolesBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            userRolesBuilder.HasKey("id");

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
