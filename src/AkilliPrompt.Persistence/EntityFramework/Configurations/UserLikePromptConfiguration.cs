using AkilliPrompt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class UserLikePromptConfiguration : IEntityTypeConfiguration<UserLikePrompt>
{
    public void Configure(EntityTypeBuilder<UserLikePrompt> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();


        // User Relationship
        builder.HasOne(x => x.User)
            .WithMany(u => u.UserLikePrompts)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // Prompt Relationship
        builder.HasOne(x => x.Prompt)
            .WithMany(p => p.UserLikePrompts)
            .HasForeignKey(x => x.PromptId)
            .IsRequired();

        // Unique Constraint for User and Prompt Combination
        builder.HasIndex(x => new { x.UserId, x.PromptId })
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
        builder.ToTable("user_like_prompts");
    }
}