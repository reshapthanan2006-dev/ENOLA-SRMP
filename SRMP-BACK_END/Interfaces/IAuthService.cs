using SRMP.DTOs;
using SRMP.DTOs.Auth;

namespace SRMP.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(
            LoginDto dto);

        Task<string?> ForgotPasswordAsync(
            ForgotPasswordDto dto);

        Task ResetPasswordAsync(
            ResetPasswordDto dto);
    }
}