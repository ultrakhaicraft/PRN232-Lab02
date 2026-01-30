namespace PRN232.FUNewsManagement.Services.DTO.Response
{
	public class LoginResponseDTO
	{
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
		public bool Success { get; set; }
		public DateTime LoginTime { get; set; }
		public int StatusCode { get; set; }
		public string? StatusMessage { get; set; }
	}
}
