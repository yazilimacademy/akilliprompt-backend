using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AkilliPrompt.Domain.Identity;
using AkilliPrompt.Domain.Settings;
using AkilliPrompt.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IAPriceTrackerApp.WebApi.Services;

public class JwtManager
{
    private readonly JwtSettings _jwtSettings;

    public JwtManager(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    // Generates a JSON Web Token (JWT) for user authentication.
    public AccessToken GenerateToken(ApplicationUser applicationUser, IList<string> roles)
    {
        // Get the access token expiration time from settings.
        var expirationInMinutes = _jwtSettings.AccessTokenExpiration;

        // Calculate the expiration date of the token.
        var expirationDate = DateTime.UtcNow.Add(expirationInMinutes);

        // Define the claims for the JWT.  These are pieces of information about the user.
        var claims = new List<Claim>
        {
            // Unique user identifier.
            new Claim("uid", applicationUser.Id.ToString()),
            // User's email address.
            new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            // User's first name.
            new Claim(JwtRegisteredClaimNames.GivenName, applicationUser.FullName.FirstName),
            // User's last name.
            new Claim(JwtRegisteredClaimNames.FamilyName, applicationUser.FullName.LastName),
            // Unique identifier for the JWT.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Expiration time of the JWT.
            new Claim(JwtRegisteredClaimNames.Exp, expirationDate.ToFileTimeUtc().ToString()),
            // Issued at time of the JWT.
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        }
        // Add user roles to the claims.
        .Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Create a symmetric security key using the secret key from settings.  This is used to sign the token.
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        // Create signing credentials using the security key and HMACSHA256 algorithm.
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // Create the JWT using the claims, expiration date, and signing credentials.
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials
        );

        // Convert the JWT to a string.
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        // Return the access token with its expiration date.
        return new AccessToken(token, expirationDate);
    }

    // Extracts the user ID from a JWT.
    public long GetUserIdFromJwt(string token)
    {
        try
        {
            // Parse the claims from the JWT.
            var claims = ParseClaimsFromJwt(token);

            // Get the user ID claim.
            var userId = claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            // Throw an exception if the user ID is not found.
            if (string.IsNullOrWhiteSpace(userId))
                throw new AuthenticationException("Invalid token");

            // Parse the user ID from the claim and return it.
            return long.Parse(userId);
        }
        catch (Exception ex)
        {
            // Throw an authentication exception if there is an error.
            throw new AuthenticationException("Invalid token", ex);
        }
    }

    // Validates a JWT.
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        try
        {
            // Validate the token using the secret key and validation parameters.  Note that ValidateLifetime is set to false.
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false, // We'll handle expiration separately.
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    // Parses the claims from a JWT payload.
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        // Split the JWT into segments and extract the payload.
        var payload = jwt.Split('.')[1];
        // Decode the base64 encoded payload.
        var jsonBytes = ParseBase64WithoutPadding(payload);
        // Deserialize the payload into a dictionary.
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        // Convert the key-value pairs into claims.
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    // Adds padding to a base64 string if necessary.
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
