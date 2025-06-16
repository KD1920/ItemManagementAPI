namespace ItemManagement.Domain.Models.ResponseModels;

public class GetUserWithPasswordResponseModel
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public int RoleId { get; set; }
	public bool? Active { get; set; }
	public DateTime? CreatedOn { get; set; }
}
