using avatarize.Services;
using Microsoft.AspNetCore.Mvc;

namespace avatarize.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvatarController : ControllerBase
    {
        private readonly AvatarGenerationService _avatarGenerationService;

        public AvatarController(AvatarGenerationService avatarGenerationService) 
            =>  _avatarGenerationService = avatarGenerationService;

        [HttpGet]
        public IActionResult Get(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return BadRequest();

            return Ok(_avatarGenerationService.GenerateAvatar(input));
        }
    }    
}