using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility.CustomException;

namespace PRN232.FUNewsManagement.API.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthenticationController : ControllerBase
	{

		private readonly IAuthenticationService _authenticationService;

		public AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
		{

			LoginResponseDTO result = await _authenticationService.ValidateUserCredentials(loginRequest.AccountEmail, loginRequest.AccountPassword);

			//Success response
			if (result.Success)
			{
				LoginResponseDTO loginResponseDTO= new LoginResponseDTO
				{
					Token = result.Token,
					Expiration = result.Expiration,
					LoginTime = result.LoginTime,
					Success = result.Success,
				};

				var response = APIResponse<LoginResponseDTO>.SuccessResponse(loginResponseDTO, "Login successful");
				return Ok(response);
			}
			//Failure response
			else
			{
				var response = APIResponse<LoginResponseDTO>.ErrorResponse(result.StatusMessage, result.StatusCode);
				return StatusCode(response.StatusCode, response);
			}



		}


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            try
            {
                await _authenticationService.Register(registerRequest);
                var response = APIResponse<string>.SuccessResponse(null, "Registration successful");
                return Ok(response);
            }
            catch (BadRequestException ex)
            {
                var response = APIResponse<string>.ErrorResponse(ex.Message, 400);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = APIResponse<string>.ErrorResponse(ex.Message, 500);
                return StatusCode(500, response);
            }
        }
	}
}
