using BookStore.API.Common.Settings;
using BookStore.API.Data;
using BookStore.API.Filters;
using BookStore.API.Middlewares;
using BookStore.API.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// by direct 
//var storeName = builder.Configuration["BookStoreSettings:StoreName"];
//var maxPageSize = builder.Configuration.GetValue<int>("BookStoreSettings:MaxPageSize");


// by use option Pattern
builder.Services.Configure<BookStoreSettings>(
    builder.Configuration.GetSection(nameof(BookStoreSettings)));

builder.Services.AddDbContext<BookStoreDbContext>(options =>
options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<BookStoreDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddValidatorsFromAssemblyContaining
    <CreateAuthorRequestValidator>();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ValidationFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
        .Where(e => e.Value?.Errors.Count > 0)
        .Select(group => new ValidationError(
                    group.Key,
                    group.Value!.Errors
                    .Select(failure => failure.ErrorMessage)
                    .Distinct()
                    .ToArray()))
        .ToArray();

        return new BadRequestObjectResult(
            ApiResponse<object?>.Failure(
                errors,
                "One or more validation errors occurred."));
    };
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

//builder.Services.AddScoped<ExceptionHandlingMiddleware>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure Middleware pipline
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();