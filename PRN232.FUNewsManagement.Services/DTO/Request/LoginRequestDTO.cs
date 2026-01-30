using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Request
{
	public class LoginRequestDTO
	{
		[Required(ErrorMessage = "Email is required")]
		public string AccountEmail { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string AccountPassword { get; set; }
	}
}
