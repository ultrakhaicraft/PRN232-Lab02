using PRN232.FUNewsManagement.Repo.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.DTO
{
	public record PagedRequest(
		int Page = 1, //Accept 1 and beyond
		int PageSize = 20,
		string? SearchKeyword = null,
		bool? IsAscending = null,
	    string? Fields = null
	);

	public record TagPagedRequest : PagedRequest
	{
		public Status? TagStatus { get; set; }

	}

	public record CommentPagedRequest : PagedRequest
	{
		public string? NewsArticleId { get; set; }

	}

	public record AccountPagedRequest : PagedRequest
	{
		public Status? AccountStatus { get; set; } 

	}

	public record NewsPagedRequest : PagedRequest
	{
		public short? CategoryId { get; set; }
	}

	public record CategoryPagedRequest : PagedRequest
	{
		public bool? IsActive { get; set; }
	}

	public record PagedResult<T>(
		int Page,
		int PageSize,
		int TotalPage,
		int TotalItems,
		IReadOnlyList<T> Items
	);

	
}
