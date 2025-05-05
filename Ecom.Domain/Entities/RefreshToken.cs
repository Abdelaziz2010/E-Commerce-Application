using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        //public string? JwtId { get; set; }                          // Track the JWT ID for the token
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Track WHEN token was created
        public DateTime ExpiredAt { get; set; }                     // Track WHEN token expires
        public bool IsExpired => DateTime.UtcNow >= ExpiredAt;      // Track if token expires or not
        public DateTime? Revoked { get; set; }                      // Track WHEN revocation occurred
        public bool IsActive => Revoked == null && !IsExpired;      // Track if token is active or not
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }
    }
}
