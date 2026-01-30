using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Utility
{
	/// <summary>
	/// Status codes representing various outcomes of API operations.
	/// </summary>
	public enum APIStatusCode
	{
		Success = 200,
		Created = 201,
		NoContent = 204,
		BadRequest = 400,
		Unauthorized = 401,
		Forbidden = 403,
		NotFound = 404,
		InternalServerError = 500
	}
}
