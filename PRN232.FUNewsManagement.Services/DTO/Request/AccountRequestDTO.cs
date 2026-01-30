using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Request
{


	public class CreateAccountDTO
	{
		[Required(ErrorMessage = "Account name is required")]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "Account name must be between 2 and 100 characters")]
		public string AccountName { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		[StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
		public string AccountEmail { get; set; }

		[Required(ErrorMessage = "Account role is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
		public int AccountRole { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(70, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 70 characters")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
			ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
		public string AccountPassword { get; set; }
	}

	public class UpdateAccountDTO
	{
		[Required(ErrorMessage = "Account name is required")]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "Account name must be between 2 and 100 characters")]
		public string AccountName { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		[StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
		public string AccountEmail { get; set; }

		[Required(ErrorMessage = "Account role is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
		public int AccountRole { get; set; }

		[StringLength(70, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 70 characters")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
			ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
		public string? AccountPassword { get; set; }  // Optional for update - null means "don't change password"

		[Required(ErrorMessage = "Account status is required")]
		[RegularExpression("^(Active|Inactive|Suspended)$", ErrorMessage = "Status must be Active, Inactive, or Suspended")]
		public string AccountStatus { get; set; }
	}

}
