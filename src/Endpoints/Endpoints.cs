using System.Net;
using Microsoft.AspNetCore.Mvc;
using Avatarize.Requests;
using Avatarize.Services;

namespace Avatarize.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/avatar", ([FromServices] AvatarGenerationService service, [AsParameters] AvatarQueryParameters request) =>  
        {
            var validationResult = request.Validate();

            if(!validationResult.IsValid)
                return Results.BadRequest(validationResult.ErrorMessages);

            var imageBytes = service.Create(request);
            
            return Results.File(imageBytes, "image/png");
        })
        .WithOpenApi()
        .WithName("Get Avatar")
        .WithSummary("Outputs an avatar image for given parameters")
        .Produces(statusCode: (int) HttpStatusCode.OK, contentType: "image/png")
        .Produces(statusCode: (int) HttpStatusCode.BadRequest)
        .CacheOutput("Expire in 10 minutes")
        .RequireRateLimiting("Get Avatar Rate Limit");
    }
}