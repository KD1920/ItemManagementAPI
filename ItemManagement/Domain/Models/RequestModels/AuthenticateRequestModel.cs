namespace ItemManagement.Domain.Models.RequestModels;

public class AuthenticateRequestModel
{
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
}
