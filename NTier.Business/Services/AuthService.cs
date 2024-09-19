using Microsoft.Extensions.Configuration;
using NTier.Business.Interfaces;
using NTier.DataAccess.Repositories;
using NTier.DataAccess.UnitOfWork;
using NTier.DataObject.DTOs.User;
using NTier.DataObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.Business.Services
{
 
        public class AuthService : IAuthService
        {
            private readonly IUserRepository _userRepository;
            private readonly IJwtToken _jwtToken;
            //private readonly IUnitOfWork _unitOfWork;

            public AuthService(IUserRepository userRepository, IJwtToken jwtToken) //, IUnitOfWork unitOfWork)
            {
                _userRepository = userRepository;
                _jwtToken = jwtToken;
               // _unitOfWork = unitOfWork;
            }

            public async Task<string> RegisterAsync(UserRegistrationDto registrationDto)
            {
                try
                {
                   // await _unitOfWork.BeginTransactionAsync();

                    var existingUser = await _userRepository.GetByEmailAsync(registrationDto.Email);
                    if (existingUser != null)
                    {
                        throw new InvalidOperationException("User already exists.");
                    }

                    var newUser = new User
                    {
                        Name = registrationDto.Name,
                        Email = registrationDto.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password)
                    };

                    await _userRepository.AddAsync(newUser);
                    //await _unitOfWork.SaveChangesAsync();
                    //await _unitOfWork.CommitAsync();

                    return _jwtToken.GenerateToken(newUser);
                }
                catch (Exception)
                {
                    //await _unitOfWork.RollbackAsync();
                    throw;
                }
            }

            public async Task<string> LoginAsync(UserLoginDto loginDto)
            {
                try
                {
                    //await _unitOfWork.BeginTransactionAsync();

                    var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                    {
                        throw new InvalidOperationException("Invalid email or password.");
                    }

                    //await _unitOfWork.SaveChangesAsync();
                    //await _unitOfWork.CommitAsync();

                    return _jwtToken.GenerateToken(user);
                }
                catch (Exception)
                {
                    //await _unitOfWork.RollbackAsync();
                    throw;
                }
            }
        }


    //public class AuthService : IAuthService
    //{
    //    private readonly IUserRepository _userRepository;
    //    private readonly IJwtToken _jwtToken;

    //    public AuthService(IUserRepository userRepository, IJwtToken jwtToken)
    //    {
    //        _userRepository = userRepository;
    //        _jwtToken = jwtToken;
    //    }

    //    public async Task<string> RegisterAsync(UserRegistrationDto registrationDto)
    //    {
    //        var existingUser = await _userRepository.GetByEmailAsync(registrationDto.Email);
    //        if (existingUser != null)
    //        {
    //            throw new InvalidOperationException("User already exists.");
    //        }

    //        var newUser = new User
    //        {
    //            Email = registrationDto.Email,
    //            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password)
    //        };

    //        await _userRepository.AddAsync(newUser);

    //        return _jwtToken.GenerateToken(newUser);
    //    }

    //    public async Task<string> LoginAsync(UserLoginDto loginDto)
    //    {
    //        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
    //        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
    //        {
    //            throw new InvalidOperationException("Invalid email or password.");
    //        }

    //        return _jwtToken.GenerateToken(user);
    //    }
    //}
}
