using Microsoft.AspNetCore.Mvc;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels.LoginModel;
using RestAPIService.Application.ViewModels.RegisterModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginTokenRequest loginRequest)
        {
            var result = await _service.Login(loginRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _service.Register(registerRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
