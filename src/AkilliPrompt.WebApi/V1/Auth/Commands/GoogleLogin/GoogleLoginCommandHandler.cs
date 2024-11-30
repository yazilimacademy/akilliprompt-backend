using AkilliPrompt.Domain.Identity;
using AkilliPrompt.Domain.ValueObjects;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using AkilliPrompt.WebApi.Models;
using Google.Apis.Auth;
using IAPriceTrackerApp.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.Results;
using AkilliPrompt.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;

public sealed class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, ResponseDto<GoogleLoginDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtManager _jwtManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSettings _jwtSettings;

    public GoogleLoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        JwtManager jwtManager,
        ApplicationDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtManager = jwtManager;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ResponseDto<GoogleLoginDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        // Get payload (validation already done by validator)
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken);

        Console.WriteLine(JsonSerializer.Serialize(payload));

        // Check if user exists
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null)
        {
            var fullName = new FullName(payload.GivenName, payload.FamilyName);

            // Register new user
            user = ApplicationUser.Create(payload.Email, fullName, isEmailConfirmed: true);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var failures = result.Errors.Select(error => new ValidationFailure(error.Code, error.Description));
                throw new ValidationException(failures);
            }

            await _userManager.AddToRoleAsync(user, "User");
        }

        // Generate JWT token
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtManager.GenerateToken(user, roles);

        // Generate refresh token
        var refreshToken = new RefreshToken(Guid.CreateVersion7().ToString(), DateTime.UtcNow.Add(_jwtSettings.RefreshTokenExpiration));

        // Store refresh token in database
        var refreshTokenEntity = new Domain.Entities.RefreshToken
        {
            Token = refreshToken.Value,
            Expires = refreshToken.ExpiresOnUtc,
            CreatedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserId = user.Id,
            Id = Guid.CreateVersion7(),
        };

        _dbContext.RefreshTokens.Add(refreshTokenEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseDto<GoogleLoginDto>.Success(
            new GoogleLoginDto(accessToken, refreshToken),
            "Login successful");

    }
}