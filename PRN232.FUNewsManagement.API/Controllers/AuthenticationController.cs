using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;

namespace PRN232.FUNewsManagement.API.Controllers
{
	public class AuthenticationController : ControllerBase
	{

		private readonly IAuthenticationService _authenticationService;

		public AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginRequestDTO loginRequest)
		{

			LoginResponseDTO result = _authenticationService.ValidateUserCredentials(loginRequest.AccountEmail, loginRequest.AccountPassword).Result;

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
	}
}
