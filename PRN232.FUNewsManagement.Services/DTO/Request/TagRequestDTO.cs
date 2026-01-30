using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Request
{

	public class CreateTagDTO
	{
		[Required(ErrorMessage = "Tag Id is required")]
		public int TagId { get; set; }
		[Required(ErrorMessage = "Tag name is required")]
		public string TagName { get; set; }
		[Required(ErrorMessage = "Note is required")]
		public string Note { get; set; }
	}

	public class UpdateTagDTO
	{
		[Required(ErrorMessage = "Tag name is required")]
		public string TagName { get; set; }
		[Required(ErrorMessage = "Note is required")]
		public string Note { get; set; }

		public string TagStatus { get; set; }
	}
}
