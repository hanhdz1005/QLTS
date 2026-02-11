using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLTS.Core.Interface;
using QLTS.Infrastructure;
using QLTS.Infrastructure.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Authorization + Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Asset.View",
        policy => policy.RequireClaim("permission", "ASSET.VIEW"));
    options.AddPolicy("Asset.Create",
        policy => policy.RequireClaim("permission", "ASSET.CREATE"));
    options.AddPolicy("Asset.Edit",
        policy => policy.RequireClaim("permission", "ASSET.EDIT"));
    options.AddPolicy("Asset.Delete",
        policy => policy.RequireClaim("permission", "ASSET.DELETE"));
});

// Swagger JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();
await InfrastructureRequistration.infrastructureConfigMidleware(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();
app.Run();
