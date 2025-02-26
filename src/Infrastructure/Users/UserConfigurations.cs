using Application.Common.Security.Authentication;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations;
using Infrastructure.Common.Persistence.Configurations.Abstracts;

using SharedKernel.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Infrastructure.Users;

internal sealed class UserConfigurations : EntityTypeConfigurationDependency<User>
{
    private sealed record UserData(
        UserId Id,
        string Name,
        Email Email,
        PasswordHash PasswordHash,
        bool IsActive,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );

    private readonly UserData _adminAccount;

    public UserConfigurations(IOptions<AdminAccountSettings> adminOptions, IPasswordHasher passwordHasher)
    {
        var adminAccountSettings = adminOptions.Value;

        _adminAccount = new UserData(
            UserId.Create(1),
            adminAccountSettings.Name,
            Email.Create(adminAccountSettings.Email),
            passwordHasher.Hash(adminAccountSettings.Password),
            true,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow
        );
    }

    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureOwnedUserAddressesTable(builder);
        ConfigureOwnedUsersRolesTable(builder);

        builder.HasData(_adminAccount);
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

    private void ConfigureOwnedUsersRolesTable(EntityTypeBuilder<User> builder)
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
                .Property("_roleId")
                .HasColumnName("id_role")
                .IsRequired();

            userRolesBuilder
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey("_roleId")
                .IsRequired();

            userRolesBuilder.Ignore(ur => ur.Role);

            userRolesBuilder.WithOwner().HasForeignKey("id_user");

            userRolesBuilder
                .Property("id_user")
                .IsRequired();

            userRolesBuilder.HasData(new
            {
                id = 1L,
                id_user = _adminAccount.Id,
                _roleId = Role.Admin.Id
            });
        });
    }
}
