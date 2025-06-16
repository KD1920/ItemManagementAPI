namespace ItemManagement.Domain.Models.RequestModels;

public class AddUserRequestModel
{
	public string Name { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public int RoleId { get; set; }
	public bool? Active { get; set; }
}
