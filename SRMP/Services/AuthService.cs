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
            RegisterDto registerDto)
        {
            // Only JobSeeker and Employer can register publicly
            if (registerDto.Role != UserRole.JobSeeker &&
                registerDto.Role != UserRole.Employer)
            {
                throw new InvalidOperationException(
                    "Only Job Seekers and Employers can register."
                );
            }

            var email = registerDto.Email
                .Trim()
                .ToLowerInvariant();

            // Check duplicate email
            if (await _authRepository.EmailExistsAsync(email))
            {
                throw new InvalidOperationException(
                    "An account with this email already exists."
                );
            }

            var user = new User
            {
                Name = registerDto.Name.Trim(),
                Email = email,
                Role = registerDto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Hash password
            user.PasswordHash = PasswordHelper.HashPassword(
                user,
                registerDto.Password
            );

            // Save user
            var createdUser =
                await _authRepository.CreateUserAsync(user);

            // Generate JWT
            var token = _jwtHelper.GenerateToken(createdUser);

            return new AuthResponseDto
            {
                UserId = createdUser.UserId,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Role = createdUser.Role.ToString(),
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto loginDto)
        {
            var email = loginDto.Email
                .Trim()
                .ToLowerInvariant();

            // Find account
            var user =
                await _authRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password."
                );
            }

            // Check active status
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "This account is inactive."
                );
            }

            // Verify password
            var passwordValid =
                PasswordHelper.VerifyPassword(
                    user,
                    user.PasswordHash,
                    loginDto.Password
                );

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password."
                );
            }

            // Generate JWT
            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }
    }
}