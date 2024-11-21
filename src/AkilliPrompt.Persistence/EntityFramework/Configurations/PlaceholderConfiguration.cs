using AkilliPrompt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliPrompt.Persistence.EntityFramework.Configurations;

public sealed class PlaceholderConfiguration : IEntityTypeConfiguration<Placeholder>
{
    public void Configure(EntityTypeBuilder<Placeholder> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Name
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);


        // Prompt Relationship
        builder.HasOne(x => x.Prompt)
            .WithMany(p => p.Placeholders)
            .HasForeignKey(x => x.PromptId)            
            .IsRequired();


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
        builder.ToTable("placeholders");
    }
}