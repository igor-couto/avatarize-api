using System.Net;
using Microsoft.AspNetCore.Mvc;
using Avatarize.Requests;
using Avatarize.Services;

namespace Avatarize.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/avatar", ([FromServices] AvatarGenerationService service, AvatarRequest request) =>  
        {
            if(!request.IsValid)
                return Results.BadRequest(request.ErrorMessages);
            
            return Results.Ok(service.Create(request));
        })
        .WithOpenApi()
        .WithName("Get Avatar")
        .WithSummary("Outputs an avatar image for given parameters")
        .Produces(statusCode: (int) HttpStatusCode.OK, contentType: "image/png")
        .CacheOutput("Expire in 10 minutes");
    }
}