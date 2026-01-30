using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility;
using PRN232.FUNewsManagement.Services.Utility.CustomException;

namespace PRN232.FUNewsManagement.API.Controllers
{
	[ApiController]
	[Route("api/news")]
	public class NewsArticleController : Controller
	{
		private readonly INewsArticleService _newsArticleService;
		private readonly IMapper _mapper;

		public NewsArticleController(INewsArticleService newsArticleService, IMapper mapper)
		{
			_newsArticleService = newsArticleService;
			_mapper = mapper;
		}



		/// <summary>
		/// Search News Articles with pagination and optional category filter
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<APIResponse<PagedResult<ViewNewsArticleDTO>>>> Search([FromQuery] NewsPagedRequest request)
		{
			
				var newsArticles = await _newsArticleService.SearchNewsArticle(request);
				var viewNews = _mapper.Map<List<ViewNewsArticleDTO>>(newsArticles.Items);
				var pagedData = new PagedResult<ViewNewsArticleDTO>(
					Page: newsArticles.Page,
					PageSize: newsArticles.PageSize,
					TotalPage: newsArticles.TotalPage,
					TotalItems: newsArticles.TotalItems,
					Items: viewNews
				);
				return Ok(APIResponse<PagedResult<ViewNewsArticleDTO>>.SuccessResponse(pagedData, "News Article retrieved successfully"));
			
		}

		[HttpGet("{newsArticleId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<ViewNewsArticleDTO>>> GetDetail(int newsArticleId)
		{
		
				var news = await _newsArticleService.GetNewsArticleById(newsArticleId);

				if (news == null)
				{
					return NotFound(APIResponse<ViewNewsArticleDTO>.ErrorResponse(
						$"News with ID {newsArticleId} not found",
						APIStatusCode.NotFound.GetHashCode()
					));
				}

				var viewNews = _mapper.Map<ViewNewsArticleDTO>(news);
				return Ok(APIResponse<ViewNewsArticleDTO>.SuccessResponse(viewNews, "News retrieved successfully"));
		
		}


		[HttpPost]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Create([FromBody] CreateNewsArticleDTO request)
		{
			
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(APIResponse<object>.ErrorResponse(
						"Validation failed",
						APIStatusCode.BadRequest.GetHashCode(),
						errors
					));
				}

				await _newsArticleService.CreateNewsArticleAsync(request);
				return Ok(APIResponse<object>.SuccessResponse(null, "News article created successfully"));
		
		}

		[HttpPut("{newsArticleId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Edit([FromRoute] int newsArticleId, [FromBody] UpdateNewsArticleDTO request)
		{
			
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(APIResponse<object>.ErrorResponse(
						"Validation failed",
						APIStatusCode.BadRequest.GetHashCode(),
						errors
					));
				}

				await _newsArticleService.UpdateNewsArticleAsync(request, newsArticleId);
				return Ok(APIResponse<object>.SuccessResponse(null, "News Article updated successfully"));
			
		}

		[HttpDelete("{newArticleId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Delete([FromRoute] int newArticleId)
		{
			
				await _newsArticleService.DeleteNewsArticleAsync(newArticleId);
				return Ok(APIResponse<object>.SuccessResponse(null, "News Article deleted successfully"));
		
		}
	}
}
