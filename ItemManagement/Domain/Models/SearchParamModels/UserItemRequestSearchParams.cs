using ItemManagement.Common.Helpers;

namespace ItemManagement.Domain.Models.SearchParamModels;

public class UserItemRequestSearchParams : CommonRequestHelper
{
	public string RequestNumber { get; set; } = null!;
	public int UserId { get; set; }
	public int StatusId { get; set; }
	public DateTime? RequestDate { get; set; }
}
