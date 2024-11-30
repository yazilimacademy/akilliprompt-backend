using AkilliPrompt.Domain.ValueObjects;

namespace AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;

public record GoogleLoginDto(AccessToken AccessToken, RefreshToken RefreshToken);
