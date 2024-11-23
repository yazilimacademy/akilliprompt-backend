using System;
using AkilliPrompt.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Seeders;

public sealed class ApplicationRoleSeeder : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        var adminRole = new ApplicationRole
        {
            Id = new Guid("019358eb-f6cb-78c6-b59c-848777da66af"),
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "019358ec-42e0-70ba-8049-655ecc8e2d2e",
        };

        var userRole = new ApplicationRole
        {
            Id = new Guid("019358ec-9d53-7785-a270-e22e10677a63"),
            Name = "User",
            NormalizedName = "USER",
            ConcurrencyStamp = "019358ec-aedc-742c-b677-a6b6bd8ef3bb",
        };

        builder.HasData(adminRole, userRole);
    }
}
