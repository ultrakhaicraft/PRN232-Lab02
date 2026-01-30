using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility;

namespace PRN232.FUNewsManagement.API.Controllers
{
	[ApiController]
	[Route("api/comments")]
	public class CommentController : ControllerBase
	{
		private readonly ICommentService _commentService;
		private readonly IMapper _mapper;

		public CommentController(ICommentService commentService, IMapper mapper)
		{
			_commentService = commentService;
			_mapper = mapper;
		}




		/// <summary>
		/// Search comments with pagination
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<APIResponse<PagedResult<ViewCommentDTO>>>> Search([FromQuery] CommentPagedRequest request)
		{

			var comment = await _commentService.SearchComment(request);
			var viewComment = _mapper.Map<List<ViewCommentDTO>>(comment.Items);
			var pagedData = new PagedResult<ViewCommentDTO>(
				Page: comment.Page,
				PageSize: comment.PageSize,
				TotalPage: comment.TotalPage,
				TotalItems: comment.TotalItems,
				Items: viewComment
			);
			return Ok(APIResponse<PagedResult<ViewCommentDTO>>.SuccessResponse(pagedData, "Comment retrieved successfully"));

		}

		[HttpGet("{commentId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<ViewCommentDTO>>> GetDetail(short commentId)
		{

			var comment = await _commentService.GetCommentById(commentId);

			if (comment == null)
			{
				return NotFound(APIResponse<ViewCommentDTO>.ErrorResponse(
					$"Comment with ID {commentId} not found",
					APIStatusCode.NotFound.GetHashCode()
				));
			}

			var viewCategory = _mapper.Map<ViewCommentDTO>(comment);
			return Ok(APIResponse<ViewCommentDTO>.SuccessResponse(viewCategory, "Comment retrieved successfully"));

		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Create([FromBody] CreatedCommentDTO request)
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

			await _commentService.CreateCommentAsync(request);
			return Ok(APIResponse<object>.SuccessResponse(null, "Comment created successfully"));

		}

		[HttpPut("{commentId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Edit(short commentId, [FromBody] UpdatedCommentDTO request)
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


			await _commentService.UpdateCommentAsync(request, commentId);
			return Ok(APIResponse<object>.SuccessResponse(null, "Comment updated successfully"));

		}

		[HttpDelete("{commentId}")]
		[Authorize]
		public async Task<ActionResult<APIResponse<object>>> Delete(short commentId)
		{

			await _commentService.DeleteCommentAsync(commentId);
			return Ok(APIResponse<object>.SuccessResponse(null, "Comment deleted successfully"));

		}
	}
}
