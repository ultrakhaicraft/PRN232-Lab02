namespace PRN232.FUNewsManagement.Services.DTO.Response
{
	public class ViewCommentDTO
	{
		public int? CommentId { get; set; }
		public string? Content { get; set; }
		public int? Likes { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? NewsArticleId { get; set; }
		public short? CreatedByAccountId { get; set; }
	}
}
