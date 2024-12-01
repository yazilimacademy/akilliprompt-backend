using AkilliPrompt.Domain.Entities;
using AkilliPrompt.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class PromptConfiguration : IEntityTypeConfiguration<Prompt>
{
    public void Configure(EntityTypeBuilder<Prompt> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Title
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        // Description
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(5000);

        // Content
        builder.Property(x => x.Content)
            .IsRequired();

        // ImageUrl
        builder.Property(x => x.ImageUrl)
            .HasMaxLength(1024)
            .IsRequired(false);

        // IsActive
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        // LikeCount
        builder.Property(x => x.LikeCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasIndex(x => x.LikeCount)
        .IsDescending()
        .HasDatabaseName("IX_Prompts_LikeCount_Desc");

        // CreatorId
        builder.HasOne<ApplicationUser>(x => x.Creator)
            .WithMany(x => x.CreatedPrompts)
            .HasForeignKey(x => x.CreatorId);

        // UserFavoritePrompts Relationship
        builder.HasMany(x => x.UserFavoritePrompts)
            .WithOne(ufp => ufp.Prompt)
            .HasForeignKey(ufp => ufp.PromptId);

        // UserLikePrompts Relationship
        builder.HasMany(x => x.UserLikePrompts)
            .WithOne(ulp => ulp.Prompt)
            .HasForeignKey(ulp => ulp.PromptId);

        // Placeholders Relationship
        builder.HasMany(x => x.Placeholders)
            .WithOne(p => p.Prompt)
            .HasForeignKey(p => p.PromptId);


        // CreatedAt
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // CreatedByUserId
        builder.Property(p => p.CreatedByUserId)
            .IsRequired(false);
        //.HasMaxLength(150);

        // ModifiedAt
        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        // ModifiedByUserId
        builder.Property(p => p.ModifiedByUserId)
            .IsRequired(false);
        //.HasMaxLength(150);

        // Table Name
        builder.ToTable("prompts");
    }
}