using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Request
{
	public class CreateCategoryDTO
	{
		[Required(ErrorMessage = "Category Id is required")]
		public short CategoryId { get; set; }
		[Required(ErrorMessage = "Category Name is required")]
		public string CategoryName { get; set; }
		[Required(ErrorMessage = "Category Description is required")]
		public string CategoryDesciption { get; set; }

		public short? ParentCategoryId { get; set; }
		[Required(ErrorMessage = "IsActive is required")]
		public bool IsActive { get; set; }
	}

	public class UpdateCategoryDTO
	{
		[Required(ErrorMessage = "Category Name is required")]
		public string CategoryName { get; set; }

		[Required(ErrorMessage = "Category Description is required")]
		public string CategoryDesciption { get; set; }

		public short? ParentCategoryId { get; set; }
		[Required(ErrorMessage = "IsActive is required")]
		public bool IsActive { get; set; }
	}
}
