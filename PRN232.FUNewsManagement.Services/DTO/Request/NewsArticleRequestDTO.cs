namespace PRN232.FUNewsManagement.Services.DTO.Request
{
	public class CreateNewsArticleDTO
	{
		public string NewsArticleId { get; set; }

		public string NewsTitle { get; set; }

		public string Headline { get; set; }

		public string NewsContent { get; set; }

		public string NewsSource { get; set; }

		public short? CategoryId { get; set; }

		public bool? NewsStatus { get; set; }

		public short? CreatedById { get; set; }
	}

	public class UpdateNewsArticleDTO
	{

		public string NewsTitle { get; set; }

		public string Headline { get; set; }

		public string NewsContent { get; set; }

		public string NewsSource { get; set; }

		public short? CategoryId { get; set; }

		public bool? NewsStatus { get; set; }

		public short? UpdatedById { get; set; }

	}
}
