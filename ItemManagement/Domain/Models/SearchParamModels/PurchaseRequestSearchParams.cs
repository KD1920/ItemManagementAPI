using ItemManagement.Common.Helpers;

namespace ItemManagement.Domain.Models.SearchParamModels;

public class PurchaseRequestSearchParams : CommonRequestHelper
{
    public int UserId { get; set; }

    public DateTime? RequestDate { get; set; }
}
