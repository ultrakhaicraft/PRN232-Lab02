using AutoMapper;
using FUNewsManagementSystemRepository.Models;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.Utility.CustomException;
using PRN232.FUNewsManagement.Repo.Enum;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Service
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IGenericRepository<SystemAccount> _accountRepository;
		private readonly TokenProvider _tokenProvider;
		private readonly IMapper _mapper;

		public AuthenticationService(IGenericRepository<SystemAccount> accountRepository, TokenProvider tokenProvider, IMapper mapper)
		{
			_accountRepository = accountRepository;
			_tokenProvider = tokenProvider;
			_mapper = mapper;
		}


		// Login method to validate user credentials
		public async Task<LoginResponseDTO> ValidateUserCredentials(string email, string password)
		{
			LoginResponseDTO result = new LoginResponseDTO();
			SystemAccount account = await _accountRepository.FirstOrDefaultAsync(p=>p.AccountEmail==email);
			if (account == null)
			{
				result.Success = false;
				result.StatusCode = APIStatusCode.NotFound.GetHashCode();
				result.StatusMessage = "Account not found with Email.";
				return result;
			}

			
			bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.AccountPassword);

			if (!isPasswordValid)
			{
				result.Success = false;
				result.StatusCode = APIStatusCode.BadRequest.GetHashCode();
				result.StatusMessage = "Incorrect Password.";
				return result;
			}

			string token = _tokenProvider.generateAccessToken(account);

			result.Token = token;
			result.Expiration = DateTime.UtcNow.AddHours(10);
			result.StatusMessage = "Login successful.";
			result.Success = true;
			result.StatusCode = APIStatusCode.Success.GetHashCode();

			return result;

		}

        public async Task Register(RegisterRequestDTO request)
        {
            var existingAccount = await _accountRepository.FirstOrDefaultAsync(x => x.AccountEmail == request.AccountEmail);
            if (existingAccount != null)
            {
                throw new BadRequestException("Email already exists.");
            }
            var account = _mapper.Map<SystemAccount>(request);

            account.AccountPassword = BCrypt.Net.BCrypt.HashPassword(request.AccountPassword);
			account.AccountRole = 3;
			account.AccountStatus = Status.Active.ToString();

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();
        }
	}
}
