using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<ApplicationUser> userManager, 
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellation = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new ConflictException($"User with email {request.Email} already exists.");
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                // ToDo: we will replace by BadRequestException 
                throw new BussinessRuleException($"User registration failed: {errors}");
            }

            var tokenResult = await _tokenService.GenerateAccessTokenAsync(
                user, cancellation);

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt
            };
        }
        public async Task<AuthResponse> LoginAsync(
            LoginRequest request, 
            CancellationToken cancellation = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new BussinessRuleException("Invalid email or password.");
            }
            if (!user.IsActive)
            {
                throw new BussinessRuleException("user is not active");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new BussinessRuleException("Invalid email or password.");
            }
            var tokenResult = await _tokenService.GenerateAccessTokenAsync(
             user, cancellation);

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt
            };
        }

    }
}
