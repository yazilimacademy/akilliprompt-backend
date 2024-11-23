using AkilliPrompt.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Seeders;

public sealed class ApplicationUserSeeder : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var adminUserId = new Guid("019358e7-cfd6-7ce0-a572-55f7859864b9");

        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            UserName = "alpertunga",
            NormalizedUserName = "ALPERTUNGA",
            Email = "alper.tunga@yazilim.academy",
            NormalizedEmail = "ALPER.TUNGA@YAZILIM.ACADEMY",
            EmailConfirmed = true,
            SecurityStamp = "0c74dcdd-892a-41a3-995d-92c8529529dc",
            ConcurrencyStamp = "b68dace3-2808-4be0-9a5d-637c5f2cfb09",
            CreatedAt = Convert.ToDateTime("2024-11-23T12:11:04+00:00"),
            CreatedByUserId = adminUserId.ToString()
        };

        // adminUser.PasswordHash = "123alper123";

        var userUserId = new Guid("019358ef-0146-7bf9-994b-880e1002a653");

        var userUser = new ApplicationUser
        {
            Id = userUserId,
            UserName = "merveeksi",
            NormalizedUserName = "MERVEEKSI",
            Email = "merveeksii61@gmail.com",
            NormalizedEmail = "MERVEEKSII61@GMAIL.COM",
            EmailConfirmed = true,
            SecurityStamp = "019358ef-213d-7ada-b02d-4e60dd64b9f9",
            ConcurrencyStamp = "019358ef-3691-7726-844a-c0d9979417f4",
            CreatedAt = Convert.ToDateTime("2024-11-23T12:11:05+00:00"),
            CreatedByUserId = userUserId.ToString()
        };

        // userUser.PasswordHash = "123merve123";

        builder.HasData(adminUser, userUser);
    }
}
