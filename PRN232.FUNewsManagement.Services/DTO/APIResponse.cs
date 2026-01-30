using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.DTO
{
	public class APIResponse<T>
	{
		public string Message { get; set; }
		public int StatusCode { get; set; }
		public T? Data { get; set; }
		public List<string>? Errors { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.UtcNow;

		public static APIResponse<T> SuccessResponse(T data, string message = "Success")
		{
			return new APIResponse<T>
			{
			
				Message = message,
				StatusCode = 200,
				Data = data
			};
		}

		public static APIResponse<T> ErrorResponse(string message, int statusCode = 400, List<string>? errors = null)
		{
			return new APIResponse<T>
			{
				Message = message,
				StatusCode = statusCode,
				Errors = errors
			};
		}
	}
}
