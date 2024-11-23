using AkilliPrompt.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Seeders;

public sealed class ApplicationUserRoleSeeder : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        var adminUserRole = new ApplicationUserRole
        {
            UserId = new Guid("019358e7-cfd6-7ce0-a572-55f7859864b9"),
            RoleId = new Guid("019358eb-f6cb-78c6-b59c-848777da66af")
        };

        var userUserRole = new ApplicationUserRole
        {
            UserId = new Guid("019358ef-0146-7bf9-994b-880e1002a653"),
            RoleId = new Guid("019358ec-9d53-7785-a270-e22e10677a63"),
        };

        builder.HasData(adminUserRole, userUserRole);
    }
}
