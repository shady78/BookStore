namespace BookStore.API.Services
{
    public class UserService : IUserService
    {
        private readonly BookStoreDbContext _context;

        public UserService(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetByEmailWithRefreshTokenAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(user => user.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<ApplicationUser?> GetByRefreshTokenAsync(
            string refreshToken, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(
                user => user.RefreshTokens.Any(rt => rt.Token == refreshToken), cancellationToken);
        }
    }
}
