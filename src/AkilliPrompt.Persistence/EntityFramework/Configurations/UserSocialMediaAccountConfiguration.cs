using AkilliPrompt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class UserSocialMediaAccountConfiguration : IEntityTypeConfiguration<UserSocialMediaAccount>
{
    public void Configure(EntityTypeBuilder<UserSocialMediaAccount> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // SocialMediaType
        builder.Property(x => x.SocialMediaType)
            .IsRequired();

        // Url
        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);


        // User Relationship
        builder.HasOne(x => x.User)
            .WithMany(u => u.UserSocialMediaAccounts)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // Unique Constraint for User and SocialMediaType Combination
        builder.HasIndex(x => new { x.UserId, x.SocialMediaType })
            .IsUnique();


        // CreatedAt
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // CreatedByUserId
        builder.Property(p => p.CreatedByUserId)
            .IsRequired(false);

        // ModifiedAt
        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        // ModifiedByUserId
        builder.Property(p => p.ModifiedByUserId)
            .IsRequired(false);

        // Table Name
        builder.ToTable("user_social_media_accounts");
    }
}