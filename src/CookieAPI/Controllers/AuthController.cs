using CookieAPI.DTOs;
using CookieAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult<UserResponseDTO>> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            var user = await _authService.RegisterUserAsync(userRegisterDTO);   
            if (user is null)
            {
                return Conflict(new { 
                    Message = "User already exists in the database" 
                }
                );
            };
            return Ok(new
            {
                Message = "User has been successfully registered in the database",
                User = user
            });
        }
        [HttpPost("log-in-user")]
        public async Task<ActionResult<TokenResponseDTO>> LoginUser(UserLoginDTO userLoginDTO)
        {
            var tokens = await _authService.LogInAsync(userLoginDTO);
            if (tokens == null)
            {
                return BadRequest($"Invalid email or password.");
            }
            SetAuthCookies(tokens);
            return Ok(new
            {
                Message = "User has successfuly logged in",
            });
        }
        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<ActionResult> DeleteUser()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guid))
            {
                return Unauthorized("Invalid user identification");
            }
            var wasDeleted = await _authService.DeleteUserAsync(guid);
            if (!wasDeleted)
            {
                return NotFound($"User not found");
            }
            return NoContent();
        }

        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guid))
            {
                return Unauthorized("Invalid user identification");
            }
            var user = await _authService.UpdateUserAsync(guid,userUpdateDTO);
            if (user is null)
            {
                return NotFound("User not found");
            }
            return Ok(new
            {
                Message = "User has been successfuly updated",
                User = user
            });
        }
        [Authorize]
        [HttpPost("log-out")]
        public async Task<ActionResult> LogOut()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guid))
            {
                return Unauthorized("User not authenticated");
            }
            var check = await _authService.LogOutUser(guid);
            if (!check)
            {
                return NotFound("User not found");
            }
            ClearAuthCookies();
            return Ok(new { Message = "User has been successfully logged out" });

        }
        [AllowAnonymous]
        [HttpPost("refresh-tokens")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshTokens()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { Message = "No refresh token provided.Please log in again." });
            }
            var tokens = await _authService.RefreshTokens(refreshToken);
            if (tokens == null)
            {
                ClearAuthCookies();
                return Unauthorized($"Invalid Refresh Token");
            }
            SetAuthCookies(tokens);
            return Ok(new
            {
                Message = "New tokens are generated.",
            });
        }
        private  void SetAuthCookies(TokenResponseDTO tokenResponseDTO)
        {
            var accessToken = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SameSite=SameSiteMode.Lax
            };
            var refreshToken = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Lax,
                Path = "/api/auth/refresh-tokens"
            };
            Response.Cookies.Append("access_token", tokenResponseDTO.AccessToken,accessToken);
            Response.Cookies.Append("refresh_token", tokenResponseDTO.RefreshToken, refreshToken);
        }
        private void ClearAuthCookies()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Path = "/api/auth/refresh-tokens"
            });
        }
    }
}
