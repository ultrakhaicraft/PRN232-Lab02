using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Services.DTO.Request
{
	public class CreatedCommentDTO
	{
	
		[Required(ErrorMessage = "Content is required")]
		public required string Content { get; set; }
		[Required(ErrorMessage = "NewsArticleId is required")]
		public string? NewsArticleId { get; set; }
		[Required(ErrorMessage = "CreatedByAccountId is required")]
		public short CreatedByAccountId { get; set; }

	}

	public class UpdatedCommentDTO
	{
		[Required(ErrorMessage = "Content is required")]
		public required string Content { get; set; }
		public int? Likes { get; set; }
	}
}
