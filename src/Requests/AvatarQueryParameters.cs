using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Avatarize.Requests;

public class AvatarQueryParameters
{
    [FromQuery(Name = "input")]
    public string Input { get; init; }

    [FromQuery(Name = "size")]
    public int? Size { get; init; } = 32;

    [FromQuery(Name = "background")]
    public bool? Background { get; init; } = true;

    [FromQuery(Name = "vignette")]
    public bool? Vignette { get; init; } = false;

    [FromQuery(Name = "gradient")]
    public bool? Gradient { get; init; } = false;

    [FromQuery(Name = "frame")]
    public bool? Frame { get; init; } = true;

    public (bool IsValid, string ErrorMessages) Validate()
    {
        var errorList = new List<string>();

        if (string.IsNullOrWhiteSpace(Input))
            errorList.Add("The input cannot be empty");

        if (Size < 24)
            errorList.Add("The image size should not be less than 24 pixels");

        if (Size > 1000)
            errorList.Add("The image size should not be greater than 1000 pixels");
        
        string errorMessages = string.Join("\n", errorList);

        bool isValid = string.IsNullOrEmpty(errorMessages);

        return (isValid, errorMessages);
    }
}