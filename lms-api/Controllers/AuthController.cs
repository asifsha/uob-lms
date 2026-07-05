using System;
using lms_api.Models.Requests;
using lms_api.Models.Responses;
using lms_service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lms_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _service.RegisterAsync(
                request.Username,
                request.Password,
                request.Role);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var token = await _service.LoginAsync(
                request.Username,
                request.Password);

            return Ok(new LoginResponse
            {
                Token = token
            });
        }
    }
}

