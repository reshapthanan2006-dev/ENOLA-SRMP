using SRMP.DTOs;
using SRMP.Helpers;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IAuthRepository authRepository,
            JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto)
        {
            // Administrator accounts cannot be created
            // through public registration.
            if (dto.Role == UserRole.Administrator)
            {
                throw new UnauthorizedAccessException(
                    "Administrator accounts cannot be registered publicly.");
            }

            var email = dto.Email.Trim().ToLowerInvariant();

            // Check for duplicate email.
            if (await _authRepository.EmailExistsAsync(email))
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = email,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Hash the password before saving.
            user.PasswordHash =
                PasswordHelper.HashPassword(
                    user,
                    dto.Password);

            // Save the user.
            await _authRepository.CreateUserAsync(user);

            // Generate JWT.
            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            // Find user by email.
            var user =
                await _authRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // Inactive users cannot log in.
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Your account is inactive.");
            }

            // Verify password.
            var passwordValid =
                PasswordHelper.VerifyPassword(
                    user,
                    user.PasswordHash,
                    dto.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // Generate JWT.
            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }
    }
}