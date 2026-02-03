using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Request
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Account name must be between 2 and 100 characters")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Password must be between 6 and 70 characters")]
        public string AccountPassword { get; set; }
    }
}
