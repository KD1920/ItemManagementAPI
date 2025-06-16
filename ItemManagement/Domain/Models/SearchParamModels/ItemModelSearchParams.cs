using ItemManagement.Common.Helpers;

namespace ItemManagement.Domain.Models.SearchParamModels;

public class ItemModelSearchParams : CommonRequestHelper
{
	public int ItemTypeId { get; set; }
}
