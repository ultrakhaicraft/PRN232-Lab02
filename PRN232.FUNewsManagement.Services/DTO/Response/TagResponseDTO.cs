using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Response
{
	public class ViewTagDTO
	{
		public int? TagId { get; set; }

		public string? TagName { get; set; }= null!;

		public string? Note { get; set; } = null!;

		public string? TagStatus { get; set; } = null!;
	}
}
