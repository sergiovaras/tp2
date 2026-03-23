using OfertasApp.Data;
using Microsoft.EntityFrameworkCore;
using OfertasApp.Helpers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
       policy => policy
            .WithOrigins("https://tp2-nu.vercel.app")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configuración de base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger con seguridad opcional
builder.Services.AddSwaggerGen(c =>
{
    // Definición de seguridad para endpoints privados
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese un token válido para endpoints privados",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // ⚠️ No agregamos AddSecurityRequirement global
    // Así Swagger no fuerza token en todos los endpoints
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// Servicios propios
builder.Services.AddScoped<MercadoLibreAuthService>();
builder.Services.AddSingleton<TokenManager>();

var app = builder.Build();

// Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
 