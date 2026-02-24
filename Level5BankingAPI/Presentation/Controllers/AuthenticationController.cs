using Application.DTOs.Responses;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _service;

        public AuthenticationController(AuthenticationService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Creates a JWT
        /// </summary>
        /// <returns>A JWT allowing access to Accounts controller</returns>
        [HttpGet]
        public ActionResult<TokenResponse> GetAuthenticationToken()
            => Ok(_service.CreateToken());
    }
}
