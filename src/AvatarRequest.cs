namespace Avatarize;

public class AvatarRequest
{
    public string Input { get; set; }

    public int? Size { get; set; }

    public bool? Background { get; set; } = true;

    public bool? Vignette { get; set; }

    public bool? Gradient { get; set; }

    public bool? Frame { get; set; } = true;

    public bool IsValid { get; set; }

    public string ErrorMessages { get; set; }

    private void Validate()
    {
        var errorList = new List<string>();

        if (string.IsNullOrWhiteSpace(Input))
            errorList.Add("The input cannot be empty");

        if (Size.HasValue)
        {
            if (Size < 24)
                errorList.Add("The image size should not be less than 24 pixels");
            if (Size > 1000)
                errorList.Add("The image size should not be greater than 1000 pixels");
        }

        ErrorMessages = string.Join("\n", errorList);

        if (string.IsNullOrEmpty(ErrorMessages))
            IsValid = true;
    }

    public static ValueTask<AvatarRequest> BindAsync(HttpContext context)
    {
        int.TryParse(context.Request.Query["size"], out var size);
        bool.TryParse(context.Request.Query["background"], out var background);
        bool.TryParse(context.Request.Query["vignette"], out var vignette);
        bool.TryParse(context.Request.Query["gradient"], out var gradient);
        bool.TryParse(context.Request.Query["frame"], out var frame);

        var avatarRequest = new AvatarRequest
        {
            Input = context.Request.Query["input"],
            Size = size,
            Background = background,
            Vignette = vignette,
            Gradient = gradient,
            Frame = frame
        };

        avatarRequest.Validate();

        return ValueTask.FromResult(avatarRequest);
    }
}