using AkilliPrompt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class UserPromptCommentConfiguration : IEntityTypeConfiguration<UserPromptComment>
{
    public void Configure(EntityTypeBuilder<UserPromptComment> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Level
        builder.Property(x => x.Level)
            .IsRequired();

        // Content
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(1000);


        // User Relationship
        builder.HasOne(x => x.User)
            .WithMany(u => u.UserPromptComments)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // Prompt Relationship
        builder.HasOne(x => x.Prompt)
            .WithMany(/*p => p.UserPromptComments*/) //Todo: Should this value be in prompt.cs?
            .HasForeignKey(x => x.PromptId)
            .IsRequired();

        // Parent Comment Relationship
        builder.HasOne(x => x.ParentComment)
            .WithMany(pc => pc.ChildComments)
            .HasForeignKey(x => x.ParentCommentId)
            .IsRequired(false);


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
        builder.ToTable("user_prompt_comments");
    }
}