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
	public interface IAccountService
	{
		public Task<ViewAccountDTO> GetAccountById(int accountId);
		public Task CreateAccountAsync(CreateAccountDTO dto);
		public Task UpdateAccountAsync(UpdateAccountDTO dto, int accountId);
		public Task DeleteAccountAsync(int accountId);

		public Task<PagedResult<ViewAccountDTO>> SearchAccount(AccountPagedRequest request);


	}
}
