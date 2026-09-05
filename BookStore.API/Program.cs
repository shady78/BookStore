using BookStore.API.Data;
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

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookService, BookService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Middleware pipline
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();