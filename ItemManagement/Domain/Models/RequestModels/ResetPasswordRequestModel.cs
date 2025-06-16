namespace ItemManagement.Domain.Models.RequestModels;

public class ResetPasswordRequestModel
{
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string NewPassword { get; set; } = null!;
	public string ConfirmPassword { get; set; } = null!;
}
