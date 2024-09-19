using NTier.DataObject.DTOs.User;
using NTier.DataObject.Entities;

namespace NTier.Business.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegistrationDto registrationDto);
        Task<string> LoginAsync(UserLoginDto loginDto);
    }
}
