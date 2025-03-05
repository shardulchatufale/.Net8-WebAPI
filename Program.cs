using DotNet8WebAPI.Entity;
using DotNet8WebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// *********************** Add services to the container ***********************

builder.Services.AddDbContext<OurHeroDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OurHeroConnectionString")));

// ✅ Register the service
builder.Services.AddScoped<IOurHeroService, OurHeroService>();

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
app.UseAuthorization();
app.MapControllers();
app.Run();
