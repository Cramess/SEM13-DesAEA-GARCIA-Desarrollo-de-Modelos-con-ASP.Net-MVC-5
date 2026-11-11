using Gimnasio.Datos;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Negocio.Mapeos;
using Gimnasio.Negocio.Servicios;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GimnasioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GimnasioDB")));
builder.Services.AddScoped<IPlanRepositorio, PlanRepositorio>();
builder.Services.AddScoped<ISocioRepositorio, SocioRepositorio>();
builder.Services.AddScoped<IPlanServicio, PlanServicio>();
builder.Services.AddScoped<ISocioServicio, SocioServicio>();
builder.Services.AddAutoMapper(
    cfg => cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"],
    typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
const string CorsPolicy = "FrontendPolicy";
var origenes = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Gimnasio API v1");
    options.DocumentTitle = "Gimnasio API";
});
app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.Run();
