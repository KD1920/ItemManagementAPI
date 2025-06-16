namespace ItemManagement.Domain.Models.ResponseModels;

public class CommonUserModel
{
	public int UserId { get; set; }
	public string Name { get; set; } = null!;
	public string Email { get; set; } = null!;
	public int RoleId { get; set; }
	public string RoleName { get; set; } = null!;
	public bool IsAdmin { get; set; }
}
