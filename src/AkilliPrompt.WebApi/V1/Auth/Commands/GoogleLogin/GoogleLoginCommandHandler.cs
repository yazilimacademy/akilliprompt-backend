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
using AkilliPrompt.Persistence.Services;
using AkilliPrompt.Domain.Constants;

namespace AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;

public sealed class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, ResponseDto<GoogleLoginDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtManager _jwtManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtSettings _jwtSettings;
    private readonly ICurrentUserService _currentUserService;

    public GoogleLoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        JwtManager jwtManager,
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtManager = jwtManager;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
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

            ThrowIfIdentityResultFailed(result);

            await _userManager.AddToRoleAsync(user, RoleConstants.UserRole);
        }

        // Generate JWT token
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtManager.GenerateToken(user, roles);

        // Generate refresh token
        var refreshToken = new RefreshToken(Guid.CreateVersion7().ToString(), DateTime.UtcNow.Add(_jwtSettings.RefreshTokenExpiration));

        // Store refresh token in database
        var refreshTokenEntity = CreateRefreshToken(user, refreshToken);

        _dbContext.RefreshTokens.Add(refreshTokenEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseDto<GoogleLoginDto>.Success(
            new GoogleLoginDto(accessToken, refreshToken),
            "Login successful");

    }

    private static void ThrowIfIdentityResultFailed(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var failures = result.Errors.Select(error => new ValidationFailure(error.Code, error.Description));
            throw new ValidationException(failures);
        }
    }

    private Domain.Entities.RefreshToken CreateRefreshToken(ApplicationUser user, RefreshToken refreshToken)
    {
        return new Domain.Entities.RefreshToken
        {
            Token = refreshToken.Value,
            Expires = refreshToken.ExpiresOnUtc,
            CreatedByIp = _currentUserService.IpAddress,
            SecurityStamp = user.SecurityStamp!,
            UserId = user.Id,
            Id = Guid.CreateVersion7(),
        };
    }
}