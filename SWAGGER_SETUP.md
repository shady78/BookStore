# إضافة Swagger إلى ASP.NET Core Web API

Swagger بيوفر صفحة نقدر من خلالها نشوف الـ endpoints ونجربها بسهولة.

## 1. تثبيت الـ package

افتح الـ Terminal داخل مجلد `BookStore.API` واكتب:

```powershell
dotnet add package Swashbuckle.AspNetCore
```

## 2. تسجيل خدمات Swagger

في ملف `Program.cs`، أضف السطرين دول بعد `builder.Services.AddControllers()`:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

## 3. إضافة Swagger Middleware

بعد `var app = builder.Build();` أضف:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

> في المشروع الحالي، احذف `builder.Services.AddOpenApi()` و `app.MapOpenApi()` لأننا هنستخدم Swashbuckle بدلًا منهما.

## 4. الشكل النهائي لملف `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## 5. تشغيل المشروع

شغّل المشروع:

```powershell
dotnet run
```

بعد التشغيل، افتح رابط المشروع وأضف `/swagger` في آخره، مثال:

```text
https://localhost:xxxx/swagger
```

هتظهر صفحة Swagger UI، ومنها تقدر تعرض وتجرب كل الـ API endpoints.
