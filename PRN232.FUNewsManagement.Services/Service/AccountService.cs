using FUNewsManagementSystemRepository.Models;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Repo.Enum;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility;
using PRN232.FUNewsManagement.Services.Utility.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Service
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<SystemAccount> _accountRepository;


		public AccountService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_accountRepository = _unitOfWork.Repository<SystemAccount>();
		}


		/// <summary>
		/// Method can perform search, sort, filter and pagination. Search are limited to one data field only.
		/// </summary>
		/// <returns></returns>
		public async Task<PagedResult<ViewAccountDTO>> SearchAccount(AccountPagedRequest request)
		{
			//Search + Filter
			IQueryable<SystemAccount> accounts = _accountRepository.Find(p =>
				(string.IsNullOrEmpty(request.SearchKeyword) ||
				p.AccountName.Contains(request.SearchKeyword)) &&
				(!request.AccountStatus.HasValue || p.AccountStatus == request.AccountStatus.Value.ToString()));

			if (accounts == null || !accounts.Any())
			{
				throw new NotFoundException("No accounts found matching the criteria.");
			}

			var totalCount = await accounts.CountAsync();


			// Sorting when needed
			if (request.IsAscending.HasValue)
			{
				accounts = request.IsAscending.Value ? accounts.OrderBy(t => t.AccountName) :
					accounts.OrderByDescending(t => t.AccountName);
			}

			// Pagination
			var pagedData = await accounts
			   .Skip((request.Page - 1) * request.PageSize)
			   .Take(request.PageSize)
			   .ToListAsync();



			var items = pagedData.Select(entity =>
			{
				var dto = new ViewAccountDTO();
				var fields = ParseFields(request.Fields);

				if (fields.Contains("accountId")) dto.AccountId = entity.AccountId;
				if (fields.Contains("accountName")) dto.AccountName = entity.AccountName;
				if (fields.Contains("accountEmail")) dto.AccountEmail = entity.AccountEmail;
				if (fields.Contains("accountRole")) dto.AccountRole = entity.AccountRole;
				if (fields.Contains("accountStatus")) dto.AccountStatus = entity.AccountStatus;
				return dto;
			}).ToList();

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			return new PagedResult<ViewAccountDTO>(
				Page: request.Page,
				PageSize: request.PageSize,
				TotalPage: totalPage,
				TotalItems: totalCount,
				Items: items
			);
		}
		public async Task<ViewAccountDTO> GetAccountById(int accountId)
		{
			short shortAccountId = Convert.ToInt16(accountId);
			SystemAccount account = await _accountRepository.GetByIdAsync(shortAccountId);

			if (account == null)
			{
				throw new NotFoundException("Account not found");
			}

			ViewAccountDTO viewAccountDTO = new ViewAccountDTO
			{
				AccountId = account.AccountId,
				AccountEmail = account.AccountEmail,
				AccountName = account.AccountName,
				AccountStatus = account.AccountStatus,
				AccountRole = account.AccountRole
			};

			return viewAccountDTO;
		}
		public async Task CreateAccountAsync(CreateAccountDTO dto)
		{
			string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.AccountPassword);

			SystemAccount newTag = new SystemAccount
			{
				AccountEmail = dto.AccountEmail,
				AccountName = dto.AccountName,
				AccountPassword = hashedPassword,
				AccountRole = dto.AccountRole,
				AccountStatus = Status.Active.ToString()

			};

			await _accountRepository.AddAsync(newTag);
			await _unitOfWork.SaveChangesAsync();


		}

		public async Task UpdateAccountAsync(UpdateAccountDTO dto, int accountId)
		{
		
				short shortAccountId = Convert.ToInt16(accountId);	
				SystemAccount account = await _accountRepository.GetByIdAsync(shortAccountId);

				if (account == null)
				{
					throw new NotFoundException($"Account with {accountId} not found");
				}

				account.AccountName = dto.AccountName;
				account.AccountEmail = dto.AccountEmail;
				account.AccountRole = dto.AccountRole;
				account.AccountPassword = dto.AccountPassword;
				account.AccountStatus = dto.AccountStatus;

				_accountRepository.Update(account);
				await _unitOfWork.SaveChangesAsync();
			

		}

		public async Task DeleteAccountAsync(int accountId)
		{
		
				short shortAccountId = Convert.ToInt16(accountId);

				SystemAccount account = await _accountRepository.GetByIdAsync(shortAccountId);

				if (account == null)
				{
					throw new NotFoundException("Account not found");
				}

				_accountRepository.Delete(account);
				await _unitOfWork.SaveChangesAsync();
		
		
		}


		private HashSet<string> ParseFields(string? fields)
		{
			if (string.IsNullOrWhiteSpace(fields))
			{
				// Return all fields by default
				return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					"accountId", "accountName", "accountEmail", "accountRole", "accountStatus"
				};
			}

			return new HashSet<string>(
				fields.Split(',').Select(f => f.Trim()),
				StringComparer.OrdinalIgnoreCase
			);
		}
	}
}
