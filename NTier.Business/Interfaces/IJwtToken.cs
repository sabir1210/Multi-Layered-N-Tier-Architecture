using NTier.DataObject.Entities;

namespace NTier.Business.Interfaces
{
    public interface IJwtToken
    {
        public string ExtractUserId();
        public string GenerateToken(User user);
        public bool VerifyJwtToken(string token);
        public DateTimeOffset? GetTokenExpiry(string token);
    }
}
