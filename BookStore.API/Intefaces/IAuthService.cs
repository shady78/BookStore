namespace BookStore.API.Intefaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellation = default);

        Task<AuthResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellation = default);

        Task<AuthResponse> RefreshTokenAsync(
            RefreshTokenRequest request , CancellationToken cancellation = default);
        Task RevokeRefreshTokenAsync(
            RevokeRefreshToken request , CancellationToken cancellation = default);

    }
}
