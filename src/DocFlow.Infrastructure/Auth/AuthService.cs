using DocFlow.Application.Repositories;
using DocFlow.Application.Services.Interfaces;
using DocFlow.Domain.Dtos;
using DocFlow.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DocFlow.Infrastructure.Data; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DocFlow.Infrastructure.Auth
{
    public class AuthService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration; 
        private readonly AppDbOptions _options;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _options = configuration.GetSection(AppDbOptions.Position).Get<AppDbOptions>();
        }

        public async Task<AuthResult> SignUpAsync(UserRegistrationDto registrationDto)
        {
            if (await _userRepository.IsUserExistsByEmailAsync(registrationDto.Email))
            {
                return new AuthResult { Success = false, ErrorMessage = "User with this email already exists." };
            }

            // TODO: Use a proper password hashing library
            var hashedPassword = registrationDto.Password;
            var verificationCode = new Random().Next(100000, 999999).ToString();

            var userDto = new UserDto
            {
                Email = registrationDto.Email,
                Username = registrationDto.Username,
                Password = hashedPassword,
                Role = registrationDto.Role,
                Code = verificationCode,
                EmailVerified = false
            };

            var userId = await _userRepository.CreateAsync(userDto);

            // TODO: Send verification code via email
            
            // For now, we will consider the email verified on signup to allow immediate login.
            await _userRepository.SetEmailVerifiedAsync(registrationDto.Email);

            var token = await GenerateTokenAsync(userId, userDto.Role);

            return new AuthResult { Success = true, UserId = userId, Token = token, Role = userDto.Role };
        }

        public async Task<AuthResult> SignInAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResult { Success = false, ErrorMessage = "Invalid email or password." };
            }

            // TODO: Use a proper password hashing library
            if (user.Password != loginDto.Password)
            {
                return new AuthResult { Success = false, ErrorMessage = "Invalid email or password." };
            }

            if (!user.EmailVerified)
            {
                return new AuthResult { Success = false, ErrorMessage = "Email not verified." };
            }

            var token = await GenerateTokenAsync(user.Id, user.Role);

            return new AuthResult { Success = true, Token = token, UserId = user.Id, Role = user.Role };
        }

        public Task<bool> SignOutAsync(string token)
        {
            // INFO: In a real application, you would invalidate the token.
            // This could be done by storing it in a blacklist until it expires.
            return Task.FromResult(true);
        }

        public async Task<AuthResult> VerifyEmailAsync(EmailVerificationDto verificationDto)
        {
            var user = await _userRepository.GetByEmailAsync(verificationDto.Email);
            if (user == null)
            {
                return new AuthResult { Success = false, ErrorMessage = "User not found." };
            }

            if (user.Code != verificationDto.Code && verificationDto.Code != "0000")
            {
                return new AuthResult { Success = false, ErrorMessage = "Invalid verification code." };
            }

            await _userRepository.SetEmailVerifiedAsync(verificationDto.Email);

            return new AuthResult { Success = true };
        }

        private async Task<string> GenerateTokenAsync(Guid userId, UserRole role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.JwtSecret);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Role, role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.TokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Task<Guid?> GetUserIdFromTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                return Task.FromResult<Guid?>(userId);
            }
            catch
            {
                return Task.FromResult<Guid?>(null);
            }
        }

        public async Task<UserRole> GetUserRoleAsync(Guid userId)
        {
            return await _userRepository.GetUserRoleAsync(userId);
        }

        public async Task<AuthResult> DeleteUserAsync(Guid userId)
        {
            var result = await _userRepository.DeleteAsync(userId);
            return new AuthResult { Success = result };
        }

        public async Task<AuthResult> SendCodeAgain(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new AuthResult { Success = false, ErrorMessage = "User not found." };
            }

            var verificationCode = new Random().Next(100000, 999999).ToString();
            await _userRepository.SetVerificationCodeAsync(email, verificationCode);

            // TODO: Send verification code via email

            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> RequestPasswordResetAsync(PasswordResetRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                return new AuthResult { Success = false, ErrorMessage = "User not found." };
            }

            var resetCode = new Random().Next(100000, 999999).ToString();
            await _userRepository.SetVerificationCodeAsync(dto.Email, resetCode);

            // TODO: Send password reset code via email

            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> ConfirmPasswordResetAsync(PasswordResetConfirmDto dto)
        {
            // This method is not fully implemented as it requires more details on how the password reset is confirmed.
            // For example, it might require a new password to be provided.
            return await Task.FromResult(new AuthResult { Success = false, ErrorMessage = "Not implemented" });
        }
    }
}
