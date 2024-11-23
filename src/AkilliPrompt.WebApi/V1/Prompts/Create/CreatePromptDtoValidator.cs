using AkilliPrompt.Persistence.EntityFramework.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AkilliPrompt.WebApi.V1.Prompts.Create;

public sealed class CreatePromptDtoValidator : AbstractValidator<CreatePromptDto>
{
    private readonly ApplicationDbContext _context;

    public CreatePromptDtoValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Başlık alanı boş bırakılamaz.")
            .MaximumLength(200)
            .WithMessage("Başlık alanı en fazla {1} karakter olabilir.")
            .MinimumLength(3)
            .WithMessage("Başlık alanı en az {1} karakter olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Açıklama alanı boş bırakılamaz.")
            .MaximumLength(5000)
            .WithMessage("Açıklama alanı en fazla {1} karakter olabilir.")
            .MinimumLength(3)
            .WithMessage("Açıklama alanı en az {1} karakter olmalıdır.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("İçerik alanı boş bırakılamaz.")
            .MaximumLength(50000)
            .WithMessage("İçerik alanı en fazla {1} karakter olabilir.")
            .MinimumLength(3)
            .WithMessage("İçerik alanı en az {1} karakter olmalıdır.");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("Durum alanı boş bırakılamaz.");

        RuleFor(x => x.CategoryIds)
            .NotEmpty()
            .WithMessage("En az bir kategori seçilmelidir.")
            .Must(CategoryIdsExist)
            .WithMessage("Seçilen kategori veya kategoriler bulunamadı.");

        RuleFor(x => x.Image)
            .Must(BeValidImage!)
            .When(x => x.Image is not null)
            .WithMessage("Resim dosyası geçerli bir resim dosyası olmalıdır.");
    }


    private bool BeValidImage(IFormFile image)
    {
        if (image == null) return false;

        var allowedExtensions = new[] { ".png", ".jpeg", ".jpg", ".webp" };

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

        return allowedExtensions.Contains(extension);
    }

    private bool CategoryIdsExist(List<Guid> categoryIds)
    {
        return _context.Categories.Any(x => categoryIds.Contains(x.Id));
    }

}
