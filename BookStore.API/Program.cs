var builder = WebApplication.CreateBuilder(args);
// Configuration => appSettings.json and appSettings.{Environment}.json and Evn variables and args
// Logging => ILogger<className>
// Dendency injection => BookService , new BookService()
// builder.Services.Add
// Hosting: Content Root , Environment
builder.Services.AddControllers();
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
app.UseAuthorization();

app.MapControllers();

app.Run();