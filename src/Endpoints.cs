namespace Avatarize;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/avatar", (AvatarGenerationService service, AvatarRequest request)
            => request.IsValid ? Results.Ok(service.Create(request)) : Results.BadRequest(request.ErrorMessages));
    }
}