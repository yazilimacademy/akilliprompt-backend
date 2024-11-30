using AkilliPrompt.Domain.Settings;
using FluentValidation;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;

public sealed class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    private readonly GoogleJsonWebSignature.ValidationSettings _googleSettings;

    public GoogleLoginCommandValidator(IOptions<GoogleAuthSettings> googleSettings)
    {
        _googleSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { googleSettings.Value.ClientId }
        };

        RuleFor(x => x.GoogleToken)
            .NotEmpty()
            .WithMessage("Google token cannot be empty.")
            .MustAsync(ValidateGoogleTokenAsync)
            .WithMessage("Invalid Google token.");
    }

    private async Task<bool> ValidateGoogleTokenAsync(string token, CancellationToken cancellationToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, _googleSettings);

            return true;
        }
        catch (InvalidJwtException ex)
        {
            return Failure($"Token validation failed: {ex.Message}");
        }
    }

    private bool Failure(string message)
    {
        // Add custom error message to validation context
        var context = new ValidationContext<GoogleLoginCommand>(null);
        context.AddFailure("GoogleToken", message);
        return false;
    }
}