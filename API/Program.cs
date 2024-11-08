using API.Extensions;
using Infraestructura.Data;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddDbContext<AHDContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"))
);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "2.0",
        Title = "All Home Designer",
        Description = "API WEB de ASP.NET Core para administrar el sitio ALL Home Designer",
        Contact = new OpenApiContact
        {
            Name = "Aplicación Web",
            Url = new Uri("https://allhomedesigner.azurewebsites.net/")
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.DefaultModelsExpandDepth(-1);
});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<AHDContext>();
        await context.Database.MigrateAsync();
        await AHDContextSeed.SeedTiposUsuariosAsync(context, loggerFactory);
        await AHDContextSeed.SeedPrivilegiosAsync(context, loggerFactory);
        await AHDContextSeed.SeedTipoUsuariosPrivilegiosAsync(context, loggerFactory);
        await AHDContextSeed.SeedFilialesAsync(context, loggerFactory);
        await AHDContextSeed.SeedSucursalesAsync(context, loggerFactory);
        await AHDContextSeed.SeedPersonasAsync(context, loggerFactory);
        await AHDContextSeed.SeedEmpresassAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var _logger = loggerFactory.CreateLogger<Program>();
        _logger.LogError(ex, "Ocurrio un error durante la migracion.");
    }
}

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
