using SRMP.DTOs;

namespace SRMP.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}