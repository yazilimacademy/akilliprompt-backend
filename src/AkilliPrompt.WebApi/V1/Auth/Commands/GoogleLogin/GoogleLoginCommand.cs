using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;

public sealed record GoogleLoginCommand(string GoogleToken) : IRequest<ResponseDto<GoogleLoginDto>>;