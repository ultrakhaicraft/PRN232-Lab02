using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Interface
{
	public interface INewsArticleService
	{
		public Task<ViewNewsArticleDTO> GetNewsArticleById(int articleId);
		public Task CreateNewsArticleAsync(CreateNewsArticleDTO dto);
		public Task UpdateNewsArticleAsync(UpdateNewsArticleDTO dto, int articleId);
		public Task DeleteNewsArticleAsync(int articleId);
		public Task<PagedResult<ViewNewsArticleDTO>> SearchNewsArticle(NewsPagedRequest request);

	}
}
