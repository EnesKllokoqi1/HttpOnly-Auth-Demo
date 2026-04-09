using CookieAPI.DTOs;

namespace CookieAPI.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO userRegistrationDTO);
        Task<TokenResponseDTO> LogInAsync(UserLoginDTO userLoginDTO);
        Task<UserResponseDTO> UpdateUserAsync(Guid userId, UserUpdateDTO userUpdateDTO);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<TokenResponseDTO> RefreshTokenAsync(string refreshToken);
        Task<bool> LogOutUser(Guid userId);

    }
}
