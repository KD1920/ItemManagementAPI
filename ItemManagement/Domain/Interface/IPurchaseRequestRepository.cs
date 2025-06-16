using ItemManagement.Common.Helpers;
using ItemManagement.Data;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Domain.Interface;

public interface IPurchaseRequestRepository
{
	Task<RecordTotalRecordResponseHelper<PurchaseRequest>> GetAllPurchaseRequestsAsync(PurchaseRequestSearchParams searchParams);
	Task<PurchaseRequest> GetPurchaseRequestByIdAsync(int id);
	Task<PurchaseRequest> AddPurchaseRequestAsync(PurchaseRequest purchaseRequest);
	Task<List<PurchaseRequestItemModel>> GetPurchaseRequestItemModelByIdAsync(int id);
	Task<PurchaseRequestItemModel> AddPurchaseRequestItemModelsAsync(PurchaseRequestItemModel purchaseRequestItemModel);
	Task<List<PurchaseRequestItemModel>> GetItemRequestItemModelByIdAsync(int id);
}
