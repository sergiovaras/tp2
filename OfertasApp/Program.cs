using OfertasApp.Data;
using Microsoft.EntityFrameworkCore;
using OfertasApp.Helpers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAReactApp",
       policy => policy
            .WithOrigins(builder.Configuration["AllowedOrigins"]?.Split(',') ?? new[] { "http://localhost:5173" })
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

app.UseCors("AllowAReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
 