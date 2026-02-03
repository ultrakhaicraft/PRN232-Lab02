using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Interface
{
	public interface IAuthenticationService
	{
		public Task<LoginResponseDTO> ValidateUserCredentials(string email, string password);
        public Task Register(RegisterRequestDTO request);
	}
}
