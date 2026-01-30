using AutoMapper;
using FUNewsManagementSystemRepository.Models;
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
	[Route("api/tags")]
	public class TagController : ControllerBase
	{
		private readonly ITagService _tagService;
		private readonly IMapper _mapper;

		public TagController(ITagService tagService, IMapper mapper)
		{
			_tagService = tagService;
			_mapper = mapper;
		}


		/// <summary>
		/// Search tags with pagination and optional status filter
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<APIResponse<PagedResult<ViewTagDTO>>>> Search([FromQuery] TagPagedRequest request)
		{
			
				var tags = await _tagService.SearchTag(request);
				var viewTagItems = _mapper.Map<List<ViewTagDTO>>(tags.Items);
				var pagedData = new PagedResult<ViewTagDTO>(
					Page: tags.Page,
					PageSize: tags.PageSize,
					TotalPage: tags.TotalPage,
					TotalItems: tags.TotalItems,
					Items: viewTagItems 
				);

				return Ok(APIResponse<PagedResult<ViewTagDTO>>.SuccessResponse(pagedData, "Tags retrieved successfully"));
			
		}

		[HttpGet("{tagId}")]
		[Authorize]

		public async Task<ActionResult<APIResponse<ViewTagDTO>>> GetDetail(int tagId)
		{
			
				var tag = await _tagService.GetTagById(tagId);
				
				if (tag == null)
				{
					return NotFound(APIResponse<ViewTagDTO>.ErrorResponse(
						$"Tag with ID {tagId} not found",
						APIStatusCode.NotFound.GetHashCode()
					));
				}

				var viewTagDetail = _mapper.Map<ViewTagDTO>(tag);

				return Ok(APIResponse<ViewTagDTO>.SuccessResponse(viewTagDetail, "Tag retrieved successfully"));
			
		}

		[HttpPost]
		//[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Create([FromBody] CreateTagDTO request)
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

				await _tagService.CreateTagAsync(request);
				return Ok(APIResponse<object>.SuccessResponse(null, "Tag created successfully"));
			
		}

		[HttpPut("{tagId}")]
		//[Authorize]

		public async Task<ActionResult<APIResponse<object>>> Edit(int tagId, [FromBody] UpdateTagDTO request)
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

				await _tagService.UpdateTagAsync(request, tagId);
				return Ok(APIResponse<object>.SuccessResponse(null, "Tag updated successfully"));
		
		}

		[HttpDelete("{tagId}")]
		public async Task<ActionResult<APIResponse<object>>> Delete(int tagId)
		{
			
				await _tagService.DeleteTagAsync(tagId);
				return Ok(APIResponse<object>.SuccessResponse(null, "Tag deleted successfully"));
			
		}

	}
}
