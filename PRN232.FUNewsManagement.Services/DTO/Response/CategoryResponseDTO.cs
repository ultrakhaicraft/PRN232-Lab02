using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Response
{
	public class ViewCategoryDTO
	{
		public short? CategoryId { get; set; }

		public string? CategoryName { get; set; }

		public string? CategoryDesciption { get; set; }

		public short? ParentCategoryId { get; set; }

		public bool? IsActive { get; set; }
	}
	
}
