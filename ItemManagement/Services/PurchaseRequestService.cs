using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Services;

public class PurchaseRequestService(
	IPurchaseRequestRepository repository,
	IGenericRepository<PurchaseRequest> genericRepository,
	IGenericRepository<User> userGenericRepository,
	IGenericRepository<ItemModel> itemGenericRepository,
	IItemModelRepository itemModelRepository,
	IMapper _mapper
) : IPurchaseRequestService
{
	public async Task<RecordTotalRecordResponseHelper<PurchaseRequestResponseModel>> GetAllPurchaseRequestsAsync(PurchaseRequestSearchParams searchParams)
	{
		var isSearchParamsValid = searchParams.PageSize > 0 && searchParams.Page > 0;
		if (!isSearchParamsValid)
			throw new ArgumentException("Please enter valid PageSize or Page.");
		if (searchParams.UserId != 0)
		{
			var isUserIdExists = await userGenericRepository.ExistsAsync(x => x.Id == searchParams.UserId);
			if (!isUserIdExists)
				throw new ArgumentException("UserId does not exists");
		}
		var data = await repository.GetAllPurchaseRequestsAsync(searchParams);
		var PRAutoMapper = new List<PurchaseRequestResponseModel>();

		foreach (var purchaseRequest in data.Records)
		{
			var request = _mapper.Map<PurchaseRequestResponseModel>(purchaseRequest);
			var itemRequestData = await repository.GetItemRequestItemModelByIdAsync(purchaseRequest.Id);
			if (itemRequestData.Count > 0)
			{
				foreach (var item in itemRequestData)
				{
					request.ItemModels.Add(_mapper.Map<PurchaseRequestItemResponseModel>(item));
				}
			}
			PRAutoMapper.Add(_mapper.Map<PurchaseRequestResponseModel>(request));
		}
		var response = new RecordTotalRecordResponseHelper<PurchaseRequestResponseModel>();
        response.Records = PRAutoMapper;
        response.TotalRecords = data.TotalRecords;
		return response;
	}

	public async Task<PurchaseRequestResponseModel> GetPurchaseRequestByIdAsync(int id)
	{
		var data = await repository.GetPurchaseRequestByIdAsync(id);
		if (data is null)
			throw new KeyNotFoundException();
		var response = _mapper.Map<PurchaseRequestResponseModel>(data);
		var itemModelData = await repository.GetPurchaseRequestItemModelByIdAsync(data.Id);
		var itemModelResponse = new List<PurchaseRequestItemResponseModel>();
		foreach (var item in itemModelData)
		{
			itemModelResponse.Add(_mapper.Map<PurchaseRequestItemResponseModel>(item));
		}
		response.ItemModels = itemModelResponse;
		return response;
	}
	public async Task<PurchaseRequestResponseModel> AddPurchaseRequestAsync(CommonUserModel user, PurchaseRequestWithItemModels purchaseRequest)
	{
		var PRAutoMapper = _mapper.Map<PurchaseRequest>(purchaseRequest);
		PRAutoMapper.UserId = user.UserId;
		PRAutoMapper.UserName = user.Name;
		PRAutoMapper.InvoiceNumber = await GenerateInvoiceNumberAsync();
		var purchaseRequestAlreadyExist = await genericRepository.ExistsAsync(x => x.InvoiceNumber == PRAutoMapper.InvoiceNumber);
		if (purchaseRequestAlreadyExist)
			throw new ArgumentException("Purchase Request With This Invoice Number Already Exists!");
		foreach (var item in purchaseRequest.ItemModels)
		{
			var isItemModelAvailable = await itemGenericRepository.ExistsAsync(x => x.Id == item.ItemModelId);
			if (!isItemModelAvailable)
				throw new ArgumentException("The specified item could not be found.");
			if (!(item.Quantity > 0))
				throw new ArgumentException("Quantity must be greater than zero.");
		}
		var data = await repository.AddPurchaseRequestAsync(PRAutoMapper);

		var response = _mapper.Map<PurchaseRequestResponseModel>(data);

		response.ItemModels = purchaseRequest.ItemModels;

		foreach (var item in purchaseRequest.ItemModels)
		{
			var itemModel = _mapper.Map<PurchaseRequestItemModelRequestModel>(item);
			itemModel.PurchaseRequestId = response.Id;
			var itemModelData = _mapper.Map<PurchaseRequestItemModel>(itemModel);
			await repository.AddPurchaseRequestItemModelsAsync(itemModelData);
			var itemModelQuantity = _mapper.Map<ItemModel>(itemModel);
			await itemModelRepository.AddItemModelQuantityAsync(itemModelQuantity);
		}
		return response;
	}

	public async Task<string> GenerateInvoiceNumberAsync()
	{
		var now = DateTime.Now;
		string month = now.ToString("MMM");
		string year = now.Year.ToString();
		string prefix = $"INV-{month}-{year}-";
		var search = new PurchaseRequestSearchParams();

		var existingInvoiceNumbers = await repository.GetAllPurchaseRequestsAsync(search);

		int maxSequence = 0;

		foreach (var invoice in existingInvoiceNumbers.Records)
		{
			string[] parts = invoice.InvoiceNumber.Split('-');
			if (parts.Length == 4 && int.TryParse(parts[3], out int sequence))
			{
				if (sequence > maxSequence)
					maxSequence = sequence;
			}
		}

		int nextSequence = maxSequence + 1;
		var invoiceNumber = $"{prefix}{nextSequence}";
		return invoiceNumber;
	}

}

