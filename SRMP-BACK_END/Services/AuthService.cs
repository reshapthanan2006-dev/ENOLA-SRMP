using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using SRMP.DTOs;
using SRMP.DTOs.Auth;
using SRMP.Helpers;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;


        public AuthService(
            IAuthRepository authRepository,
            JwtHelper jwtHelper,
            IEmailService emailService,
            ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
            _emailService = emailService;
            _logger = logger;
        }


        // =====================================================
        // REGISTER
        // =====================================================

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto)
        {
            if (dto.Role == UserRole.Administrator)
            {
                throw new UnauthorizedAccessException(
                    "Administrator accounts cannot be registered publicly.");
            }

            var email =
                dto.Email
                    .Trim()
                    .ToLowerInvariant();


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


            user.PasswordHash =
                PasswordHelper.HashPassword(
                    user,
                    dto.Password);


            await _authRepository
                .CreateUserAsync(user);


            var token =
                _jwtHelper.GenerateToken(user);


            return new AuthResponseDto
            {
                UserId = user.UserId,

                FullName = user.FullName,

                Email = user.Email,

                Role = user.Role.ToString(),

                Token = token
            };
        }


        // =====================================================
        // LOGIN
        // =====================================================

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto)
        {
            var email =
                dto.Email
                    .Trim()
                    .ToLowerInvariant();


            var user =
                await _authRepository
                    .GetByEmailAsync(email);


            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }


            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Your account is inactive.");
            }


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


            var token =
                _jwtHelper.GenerateToken(user);


            return new AuthResponseDto
            {
                UserId = user.UserId,

                FullName = user.FullName,

                Email = user.Email,

                Role = user.Role.ToString(),

                Token = token
            };
        }


        // =====================================================
        // FORGOT PASSWORD
        // =====================================================

        public async Task<string?> ForgotPasswordAsync(
            ForgotPasswordDto dto)
        {
            var email =
                dto.Email
                    .Trim()
                    .ToLowerInvariant();


            var user =
                await _authRepository
                    .GetByEmailAsync(email);


            /*
             * Email exists-aa illaya nu
             * public response-la reveal panna koodadhu.
             */
            if (user == null || !user.IsActive)
            {
                _logger.LogInformation(
                    "Password reset requested for unavailable account.");

                return null;
            }


            var resetToken =
                GenerateSecureResetToken();


            var tokenHash =
                HashResetToken(resetToken);


            user.ResetPasswordTokenHash =
                tokenHash;


            user.ResetPasswordTokenExpiresAt =
                DateTime.UtcNow.AddMinutes(30);


            await _authRepository
                .UpdateUserAsync(user);


            try
            {
                await _emailService
                    .SendPasswordResetEmailAsync(
                        user.Email,
                        user.FullName,
                        resetToken);


                _logger.LogInformation(
                    "Password reset email sent successfully to {Email}.",
                    user.Email);
            }
            catch (Exception ex)
            {
                /*
                 * Mail send fail aana generated token
                 * valid-aa DB-la leave panna vendam.
                 */
                user.ResetPasswordTokenHash = null;

                user.ResetPasswordTokenExpiresAt = null;


                await _authRepository
                    .UpdateUserAsync(user);


                /*
                 * Actual SMTP error backend terminal-la
                 * display aagum.
                 */
                _logger.LogError(
                    ex,
                    "Password reset email failed for {Email}.",
                    user.Email);


                return null;
            }


            return resetToken;
        }


        // =====================================================
        // RESET PASSWORD
        // =====================================================

        public async Task ResetPasswordAsync(
            ResetPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new InvalidOperationException(
                    "Invalid or expired password reset token.");
            }


            var tokenHash =
                HashResetToken(dto.Token);


            var user =
                await _authRepository
                    .GetByResetPasswordTokenHashAsync(
                        tokenHash);


            if (user == null)
            {
                throw new InvalidOperationException(
                    "Invalid or expired password reset token.");
            }


            if (
                user.ResetPasswordTokenExpiresAt == null ||
                user.ResetPasswordTokenExpiresAt <= DateTime.UtcNow
            )
            {
                user.ResetPasswordTokenHash = null;

                user.ResetPasswordTokenExpiresAt = null;


                await _authRepository
                    .UpdateUserAsync(user);


                throw new InvalidOperationException(
                    "Invalid or expired password reset token.");
            }


            user.PasswordHash =
                PasswordHelper.HashPassword(
                    user,
                    dto.NewPassword);


            /*
             * Password reset successful.
             * Reset token one-time use only.
             */
            user.ResetPasswordTokenHash = null;

            user.ResetPasswordTokenExpiresAt = null;


            await _authRepository
                .UpdateUserAsync(user);


            _logger.LogInformation(
                "Password reset completed successfully for user {UserId}.",
                user.UserId);
        }


        // =====================================================
        // GENERATE RESET TOKEN
        // =====================================================

        private static string GenerateSecureResetToken()
        {
            var randomBytes =
                RandomNumberGenerator.GetBytes(32);


            return Convert
                .ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }


        // =====================================================
        // HASH RESET TOKEN
        // =====================================================

        private static string HashResetToken(
            string token)
        {
            var tokenBytes =
                Encoding.UTF8.GetBytes(token);


            var hashBytes =
                SHA256.HashData(tokenBytes);


            return Convert.ToHexString(
                hashBytes);
        }
    }
}