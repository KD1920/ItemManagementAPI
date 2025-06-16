using ItemManagement.Common.Helpers;

namespace ItemManagement.Domain.Models.SearchParamModels;

public class UserItemSearchParams : CommonRequestHelper
{
	public int UserId { get; set; }
	public int ItemModelId { get; set; }
}
