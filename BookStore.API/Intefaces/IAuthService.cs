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
    }
}
