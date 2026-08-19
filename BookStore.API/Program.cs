using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
// args => dotnet run --urls "http://localhost:6060"
// Configuration (appSettings.json) , appSettings.{Environment}.json
// Environment variables , args 
// ILogger<T>
// Add services to the container.
// Register Services
//builder.Services.AddScoped<ReqestTimingMiddleware>();
builder.Services.AddControllers();
//builder.Services.AddScoped<ILogger, Logger>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configure Middleware pipline
app.UseHttpsRedirection();
app.UseMiddleware<ReqestTimingMiddleware>(); // object.InvokeAsync
app.UseAuthorization();

app.MapControllers();

app.Run();


// when make run
// restore and build 

// Middleware 
// 1. Convention-based middleware
// 2. Factory-based Middleware 
public class ReqestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ReqestTimingMiddleware> _logger;

    public ReqestTimingMiddleware(RequestDelegate next, ILogger<ReqestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);
        stopwatch.Stop();

        _logger.LogInformation(
            "{Method} {Path} took {elapsedMilliseconds} ms",
            context.Request.Method,
            context.Request.Path,
            stopwatch.ElapsedMilliseconds);
    }
}

// when run project before make any request 
// create one object of each Conventional Middleware
// because this reason can inject Singleton Service in the middleware constructor
// but scoped can inject in the InvokeAsync method of middleware
// we have another type of middleware called Factory-based Middleware
// this middleware can inject scoped service in the constructor
// but it is not recommended because it will create a new instance of the middleware for each request
// but we can use it when we want to inject scoped service in the constructor of the middleware


/******what has happened when receiving a request *******/
// 1. The request is received by the web server (Kestrel).
//// 2. create object (HttpContext)
//HttpContext
//├── Request
//│   ├── Method
//│   ├── Path
//│   ├── Headers
//│   └── Body
//│
//├── Response
//│   ├── StatusCode
//│   ├── Headers
//│   └── Body
//│
//├── User
//├── TraceIdentifier
//└── RequestServices
// every request has a different HttpContext object