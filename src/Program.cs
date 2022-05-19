using Microsoft.OpenApi.Models;
using Avatarize;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.0.0",
        new OpenApiInfo
        {
            Title = "Avatarize API",
            Description = "Generate user avatars using hash visualization techniques.",
            Version = "v1.0.0"
        });
});

builder.Services.AddSingleton(new AssetsService());
builder.Services.AddTransient<AvatarGenerationService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<ImageService>();

await using var app = builder.Build();

app.UseHttpsRedirection();
app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "Avatarize API v1.0.0"); });
    app.UseDeveloperExceptionPage();
}

app.MapEndpoints();

app.Run();

public partial class Program { }