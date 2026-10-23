using BookStore.API.Common.Interfaces;

namespace BookStore.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? Email => 
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);


        public bool IsAuthenticated =>
           _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
