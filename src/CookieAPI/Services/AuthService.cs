using CookieAPI.Data;
using CookieAPI.DTOs;
using CookieAPI.Interfaces;
using CookieAPI.Entities;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

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

        public Task<TokenResponseDTO> GenerateTokens(User user)
        {
            throw new NotImplementedException();
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
                expires: DateTime.UtcNow.AddMinutes(45),
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
        public Task<bool> DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponseDTO> LogInAsync(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogOutUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO userRegistrationDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDTO> UpdateUserAsync(Guid userId, UserUpdateDTO userUpdateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
