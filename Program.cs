using DotNet8WebAPI.Entity;
using DotNet8WebAPI.Helper;
using DotNet8WebAPI.Model;
using DotNet8WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// *********************** Add services to the container ***********************

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings")); 

builder.Services.AddDbContext<OurHeroDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OurHeroConnectionString")));

// ✅ Register the service
builder.Services.AddScoped<IOurHeroService, OurHeroService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Project Structure",
        Version = "v1" 
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat="JWT"

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
            new List<string>()
        }
    });
});

// *********************** Add services to the container end ***********************

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<jwtMiddleware>(); 
app.UseAuthorization();
app.MapControllers();
app.Run();

