using ItemManagement.Common.Helpers;
using ItemManagement.Data;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services.Interfaces;

public interface IPurchaseRequestService
{
	Task<RecordTotalRecordResponseHelper<PurchaseRequestResponseModel>> GetAllPurchaseRequestsAsync(PurchaseRequestSearchParams searchParams);
	Task<PurchaseRequestResponseModel> GetPurchaseRequestByIdAsync(int id);
	Task<PurchaseRequestResponseModel> AddPurchaseRequestAsync(CommonUserModel user, PurchaseRequestWithItemModels purchaseRequest);
}
