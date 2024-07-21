using Avatarize.Configuration;
using Avatarize.Endpoints;
using Avatarize.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthCheck();
builder.Services.AddVersioning();
builder.Services.AddCache();
builder.Services.AddCorsConfiguration();

builder.Services.AddSingleton(new AssetsService());
builder.Services.AddTransient<AvatarGenerationService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<ImageService>();

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseHealthCheckConfiguration();
app.UseSwaggerConfiguration();
app.UseDeveloperExceptionPage();
app.MapEndpoints();
app.UseCorsConfiguration();

app.Run();

public partial class Program { }