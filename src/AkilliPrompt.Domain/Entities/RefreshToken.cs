using AkilliPrompt.Domain.Common;
using AkilliPrompt.Domain.Identity;

namespace AkilliPrompt.Domain.Entities;
public class RefreshToken : EntityBase
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string CreatedByIp { get; set; }
    public string SecurityStamp { get; set; }
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
}