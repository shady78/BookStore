using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AuthService(UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IUserService userService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userService = userService;
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

            return await CreateAuthResponseAsync(user, cancellation);
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
            return await CreateAuthResponseAsync(user, cancellation);
        }

        public async Task<AuthResponse> RefreshTokenAsync(
            RefreshTokenRequest request,
            CancellationToken cancellation = default)
        {
            var user = await _userService.GetByRefreshTokenAsync(
                request.RefreshToken, cancellation);

            if (user is null)
            {
                throw new BussinessRuleException("Invalid Refresh token.");
            }

            var oldRefreshToken = user.RefreshTokens
                .First(rt => rt.Token == request.RefreshToken);

            if (!oldRefreshToken.IsActive)
            {
                throw new BussinessRuleException("Refresh token is expired or revoked.");
            }
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            oldRefreshToken.RevokedAt = DateTime.UtcNow;

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var tokenResult = await _tokenService.GenerateAccessTokenAsync(
             user, cancellation);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = $"{user.FirstName} {user.LastName}",
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            };
        }

        // ToDo: return bool data (true)
        public async Task RevokeRefreshTokenAsync(
            RevokeRefreshToken request,
            CancellationToken cancellation = default)
        {
            var user = await _userService.GetByRefreshTokenAsync(
                request.RefreshToken, cancellation);

            if (user is null)
            {
                throw new BussinessRuleException("Invalid refresh token.");
            }
            var refreshToken = user.RefreshTokens
                .First(rt => rt.Token == request.RefreshToken);

            if (!refreshToken.IsActive)
            {
                return;
            }
            if (refreshToken.IsExpired)
            {
                return;
            }
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        // helper method
        private async Task<AuthResponse> CreateAuthResponseAsync(
            ApplicationUser user, CancellationToken cancellation)
        {
            var tokenResult = await _tokenService.GenerateAccessTokenAsync(
               user, cancellation);

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return new AuthResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            };

        }
    }
}
