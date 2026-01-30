using AutoMapper;
using Azure;
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
	[Route("api/accounts")]
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;
		private readonly IMapper _mapper;

		public AccountController(IAccountService accountService, IMapper mapper)
		{
			_accountService = accountService;
			_mapper = mapper;
		}



		/// <summary>
		/// Search accounts with pagination, sorting and filtering
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]

		public async Task<ActionResult<APIResponse<PagedResult<ViewAccountDTO>>>> Search([FromQuery] AccountPagedRequest request)
		{

			var accounts = await _accountService.SearchAccount(request);
			var viewAccounts = _mapper.Map<List<ViewAccountDTO>>(accounts.Items);
			var pagedData = new PagedResult<ViewAccountDTO>(
				Page: accounts.Page,
				PageSize: accounts.PageSize,
				TotalPage: accounts.TotalPage,
				TotalItems: accounts.TotalItems,
				Items: viewAccounts
			);
			return Ok(APIResponse<PagedResult<ViewAccountDTO>>.SuccessResponse(pagedData, "Accounts retrieved successfully"));

		}

		[HttpGet("{accountId}")]
		[Authorize]

		public async Task<ActionResult<APIResponse<ViewAccountDTO>>> GetDetail(int accountId)
		{
			
				var account = await _accountService.GetAccountById(accountId);

				if (account == null)
				{
					return NotFound(APIResponse<ViewAccountDTO>.ErrorResponse(
						$"Account with ID {accountId} not found",
						statusCode: APIStatusCode.NotFound.GetHashCode()
					));
				}
				var viewAccountDetail = _mapper.Map<ViewAccountDTO>(account);
				return Ok(APIResponse<ViewAccountDTO>.SuccessResponse(viewAccountDetail, "Account retrieved successfully"));
			
		}

		[HttpPost]
		[Authorize]

		public async Task<ActionResult<APIResponse<object>>> Create([FromBody] CreateAccountDTO request)
		{

				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(APIResponse<object>.ErrorResponse(
						"Validation failed",
						statusCode: APIStatusCode.BadRequest.GetHashCode(),
						errors
					));
				}

				await _accountService.CreateAccountAsync(request);
				return Ok(APIResponse<object>.SuccessResponse(null, "Account created successfully"));
			
			
		}


		[HttpPut("{accountId}")]
		[Authorize]

		public async Task<ActionResult<APIResponse<object>>> Edit(int accountId, [FromBody] UpdateAccountDTO request)
		{
		
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(APIResponse<object>.ErrorResponse(
						"Validation failed",
						400,
						errors
					));
				}

				await _accountService.UpdateAccountAsync(request, accountId);
				return Ok(APIResponse<object>.SuccessResponse(null, "Account updated successfully"));
			
		}

		[HttpDelete("{accountId}")]
		[Authorize]

		public async Task<ActionResult<APIResponse<object>>> Delete(int accountId)
		{
			
				await _accountService.DeleteAccountAsync(accountId);
				return Ok(APIResponse<object>.SuccessResponse(null, "Account deleted successfully"));
			
		}
	}
}
