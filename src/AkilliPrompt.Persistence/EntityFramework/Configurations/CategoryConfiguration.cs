using AkilliPrompt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Name
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Description
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(500);


        // PromptCategories Relationship
        builder.HasMany(x => x.PromptCategories)
            .WithOne(pc => pc.Category)
            .HasForeignKey(pc => pc.CategoryId);


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
        builder.ToTable("categories");
    }
}