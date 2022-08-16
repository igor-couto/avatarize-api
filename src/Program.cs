using Avatarize;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;

if (environment.IsDevelopment())
{
    builder.Services.AddSwagger();
    builder.Services.AddEndpointsApiExplorer();
}

builder.Services.AddHealthChecks();

builder.Services.AddSingleton(new AssetsService());
builder.Services.AddTransient<AvatarGenerationService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<ImageService>();

await using var app = builder.Build();

app.UseHttpsRedirection();
app.MapHealthChecks("/health");

if (environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
    app.UseDeveloperExceptionPage();
}

app.MapEndpoints();

app.Run();

public partial class Program { }