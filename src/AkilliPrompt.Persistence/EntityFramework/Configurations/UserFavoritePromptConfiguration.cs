using AkilliPrompt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class UserFavoritePromptConfiguration : IEntityTypeConfiguration<UserFavoritePrompt>
{
    public void Configure(EntityTypeBuilder<UserFavoritePrompt> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();


        // User Relationship
        builder.HasOne(x => x.User)
            .WithMany(u => u.UserFavoritePrompts)
            .HasForeignKey(x => x.UserId);

        // Prompt Relationship
        builder.HasOne(x => x.Prompt)
            .WithMany(p => p.UserFavoritePrompts)
            .HasForeignKey(x => x.PromptId);

        // Unique Constraint for User and Prompt Combination
        builder.HasIndex(x => new { x.UserId, x.PromptId })
            .IsUnique(); //BUNU KONTROL ETMELİYİZ


        // CreatedAt
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // CreatedByUserId
        builder.Property(p => p.CreatedByUserId)
            .IsRequired(false)
            .HasMaxLength(100);

        // ModifiedAt
        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        // ModifiedByUserId
        builder.Property(p => p.ModifiedByUserId)
            .IsRequired(false)
            .HasMaxLength(100);

        // Table Name
        builder.ToTable("user_favorite_prompts");
    }
}