namespace BookStore.API.Intefaces
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetByEmailWithRefreshTokenAsync(
            string email, CancellationToken cancellationToken = default);

        Task<ApplicationUser?> GetByRefreshTokenAsync(
            string refreshToken, CancellationToken cancellationToken = default);


    }
}
