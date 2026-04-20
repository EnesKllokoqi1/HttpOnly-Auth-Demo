using CookieAPI.Data;
using CookieAPI.DTOs;
using CookieAPI.Entities;
using CookieAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CookieAPI.Services
{
    public class AuthService : IAuthService
    {
        public IConfiguration _configuration;
        public AppDbContext _appDbContext;
        public AuthService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext =appDbContext;
            _configuration =configuration;
        }

        public async Task<TokenResponseDTO> GenerateTokens(User user)
        {
            return new TokenResponseDTO {
               AccessToken = GenerateJwtToken(user),
               RefreshToken = await GenerateRefreshToken(user)
            };
        
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Guid.ToString()),
                new Claim(ClaimTypes.Email,user.EmailAddress.ToString()),
                new Claim(ClaimTypes.Gender,user.Gender.ToString()!),
            };
            var signinCreds = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer:_configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims:claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials:signinCreds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        private async Task<string> GenerateRefreshToken(User user)
        {

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.RefreshToken=refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _appDbContext.SaveChangesAsync();
            return refreshToken;
        }
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<TokenResponseDTO> LogInAsync(UserLoginDTO userLoginDTO)
        {
            var normalisedEmail = userLoginDTO.EmailAddress.Trim().ToLower();
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e => e.EmailAddress == normalisedEmail);
            if (user is null){return null;}
            if (!BCrypt.Net.BCrypt.Verify(userLoginDTO.Password,user.PasswordHash))
            {
                return null;
            }
            return await GenerateTokens(user);

        }

        public async Task<bool> LogOutUser(Guid userId)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TokenResponseDTO> RefreshTokens(string refreshToken)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);
            if (user == null)
            {
                return null;
            }
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _appDbContext.SaveChangesAsync();
                return null;
            }
            return await GenerateTokens(user);
        }

        public async Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO userRegistrationDTO)
        {
            var normalisedEmail = userRegistrationDTO.EmailAddress.Trim().ToLower();
            var check = await _appDbContext.Users.FirstOrDefaultAsync(e=>e.EmailAddress==normalisedEmail);
            if (check != null)
            {
                return null;
            }
            var user = new User
            {
                FirstName = userRegistrationDTO.FirstName,
                LastName = userRegistrationDTO.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationDTO.Password),
                EmailAddress=userRegistrationDTO.EmailAddress,
                Gender=userRegistrationDTO.Gender,
                Age = userRegistrationDTO.Age
            };
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return MapToUserDTO(user);
        }

        public async Task<UserResponseDTO> UpdateUserAsync(Guid userId, UserUpdateDTO userUpdateDTO)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            if (user is null)
            {
                return null;
            }
            user.FirstName = userUpdateDTO.FirstName;
            user.LastName = userUpdateDTO.LastName;
            user.Gender = userUpdateDTO.Gender;
            user.Age = userUpdateDTO.Age;
            await _appDbContext.SaveChangesAsync();
            return MapToUserDTO(user);

        }
        private UserResponseDTO MapToUserDTO(User user)
        {
            return new UserResponseDTO
            {
                FullName=$"{user.FirstName}{user.LastName}",
                EmailAddress=user.EmailAddress,
                Gender=user.Gender,
                Age=user.Age,
            };
        }
    }
}
