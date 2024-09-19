using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTier.Business.Interfaces;
using NTier.Business.Services;
using NTier.DataObject.DTOs.User;

namespace NTier.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
                return Ok("fdfsd");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            try
            {
                var token = await _authService.RegisterAsync(registrationDto);
                return Ok(new { Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
