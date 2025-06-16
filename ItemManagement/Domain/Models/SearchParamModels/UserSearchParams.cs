using ItemManagement.Common.Helpers;

namespace ItemManagement.Domain.Models.SearchParamModels;

public class UserSearchParams : CommonRequestHelper
{
	public int RoleId { get; set; }
	public bool? Active { get; set; }
}
