namespace BookStore.API.Intefaces
{
    public interface ITokenService
    {
        Task<(string Token, DateTime ExpiresAt)> GenerateAccessTokenAsync
            (ApplicationUser user, CancellationToken cancellation = default);
    }
}
