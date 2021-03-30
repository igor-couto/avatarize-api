using Avatarize.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avatarize.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvatarController : ControllerBase
    {
        private readonly AvatarGenerationService _avatarGenerationService;

        public AvatarController(AvatarGenerationService avatarGenerationService) 
            =>  _avatarGenerationService = avatarGenerationService;

        [HttpGet]
        public IActionResult Get(
            [FromQuery] AvatarQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Input))
                return BadRequest("The input cannot be empty");

            return Ok(_avatarGenerationService.GenerateAvatar(query));
        }
    }  
}