using AutoMapper;
using Azure.Core;
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
	[Route("api/categories")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryService categoryService, IMapper mapper)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}





		/// <summary>
		/// Search categories with pagination
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<APIResponse<PagedResult<ViewCategoryDTO>>>> Search([FromQuery] CategoryPagedRequest request)
		{
			
				var category = await _categoryService.SearchCategory(request);
				var viewCategory = _mapper.Map<List<ViewCategoryDTO>>(category.Items);
				var pagedData = new PagedResult<ViewCategoryDTO>(
					Page: category.Page,
					PageSize: category.PageSize,
					TotalPage: category.TotalPage,
					TotalItems: category.TotalItems,
					Items: viewCategory
				);
				return Ok(APIResponse<PagedResult<ViewCategoryDTO>>.SuccessResponse(pagedData, "Category retrieved successfully"));
		
		}

		[HttpGet("{categoryId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<ViewCategoryDTO>>> GetDetail(short categoryId)
		{
		
				var category = await _categoryService.GetCategoryById(categoryId);

				if (category == null)
				{
					return NotFound(APIResponse<ViewCategoryDTO>.ErrorResponse(
						$"Category with ID {categoryId} not found",
						APIStatusCode.NotFound.GetHashCode()
					));
				}

				var viewCategory = _mapper.Map<ViewCategoryDTO>(category);
				return Ok(APIResponse<ViewCategoryDTO>.SuccessResponse(viewCategory, "Category retrieved successfully"));
			
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Create([FromBody] CreateCategoryDTO request)
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

				await _categoryService.CreateCategoryAsync(request);
				return Ok(APIResponse<object>.SuccessResponse(null, "Category created successfully"));
			
		}

		[HttpPut("{categoryId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Edit(short categoryId, [FromBody] UpdateCategoryDTO request)
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


				await _categoryService.UpdateCategoryAsync(request, categoryId);
				return Ok(APIResponse<object>.SuccessResponse(null, "Category updated successfully"));
		
		}

		[HttpDelete("{categoryId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Delete(short categoryId)
		{
			
				await _categoryService.DeleteCategoryAsync(categoryId);
				return Ok(APIResponse<object>.SuccessResponse(null, "Category deleted successfully"));
			
		}
	}
}
