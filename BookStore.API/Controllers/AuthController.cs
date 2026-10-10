namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(
            RegisterRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(ApiResponse<AuthResponse>
                .Success(response, "user registered successfully."));
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(
            LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(ApiResponse<AuthResponse>
                .Success(response, "User logged in successfully."));
        }


    }
}
