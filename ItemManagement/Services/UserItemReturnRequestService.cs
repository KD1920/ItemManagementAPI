using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Common.Service.Interface;
using ItemManagement.ApplicationConstants.Enums;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Services;

public class UserItemReturnRequestService(
	IUserItemReturnRequestRepository repository,
	IGenericRepository<ItemModel> genericRepository,
	IGenericRepository<ReturnStatus> returnRequestGenericRepository,
	IGenericRepository<User> userGenericRepository,
	IGenericRepository<ItemReturnRequest> itemReturnRequestGenericRepository,
	IMapper _mapper,
	IItemModelRepository itemModelRepository,
	IUserItemRepository userItemRepository,
	ICommonService commonService
	) : IUserItemReturnRequestService
{
	public async Task<RecordTotalRecordResponseHelper<UserItemRequestResponseModel>> GetAllItemReturnRequestsAsync(UserItemRequestSearchParams searchParams)
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
		var user = await commonService.GetUser();
		if (searchParams.StatusId != 0)
		{
			var isStatusExists = await IsStatusExists(searchParams.StatusId);
			if (!isStatusExists)
				throw new ArgumentException("Status does not exists");
		}
		var data = await repository.GetAllItemReturnRequestsAsync(searchParams);
		var IRAutoMapper = new List<UserItemRequestResponseModel>();
		foreach (var itemRequest in data.Records)
		{
			var request = _mapper.Map<UserItemRequestResponseModel>(itemRequest);
			if (user.UserId == request.UserId || user.IsAdmin)
			{
				if (request.UserId == user.UserId || user.IsAdmin)
				{
					var itemRequestData = await repository.GetItemReturnRequestItemModelByIdAsync(itemRequest.Id);
					if (itemRequestData.Count > 0)
					{
						foreach (var item in itemRequestData)
						{
							request.ItemModels.Add(_mapper.Map<PurchaseRequestItemResponseModel>(item));
						}
					}
					IRAutoMapper.Add(_mapper.Map<UserItemRequestResponseModel>(request));
				}
			}
		}
		var response = new RecordTotalRecordResponseHelper<UserItemRequestResponseModel>();
        response.Records = IRAutoMapper;
        response.TotalRecords = data.TotalRecords;
		return response;
	}

	public async Task<UserItemRequestResponseModel> GetItemReturnRequestByIdAsync(int id)
	{
		var user = await commonService.GetUser();
		var data = await repository.GetItemReturnRequestByIdAsync(id);
		if (data is null)
			throw new KeyNotFoundException();
		if (user.UserId != data.UserId)
			return null;
		var response = _mapper.Map<UserItemRequestResponseModel>(data);
		var itemModelData = await repository.GetItemReturnRequestItemModelByIdAsync(data.Id);
		if (itemModelData.Count > 0)
		{
			foreach (var item in itemModelData)
			{
				response.ItemModels.Add(_mapper.Map<PurchaseRequestItemResponseModel>(item));
			}
		}
		return response;
	}

	public async Task<UserItemRequestResponseModel> AddItemReturnRequestAsync(
		AddUserItemWithQuantityRequestModel itemRequest,
		int statusId,
		int userId
	)
	{
		var IRAutoMapper = _mapper.Map<ItemReturnRequest>(itemRequest);
		IRAutoMapper.RequestNumber = await GenerateRequestNumberAsync();
		IRAutoMapper.StatusId = statusId;
		IRAutoMapper.UserId = userId;
		if (!(itemRequest.ItemModels.Count > 0))
			throw new ArgumentException("At least one item must be included in the request.");

		var data = await repository.AddItemReturnRequestAsync(IRAutoMapper);

		var response = _mapper.Map<UserItemRequestResponseModel>(data);

		response.ItemModels = itemRequest.ItemModels;

		foreach (var item in itemRequest.ItemModels)
		{
			var isItemModelAvailable = await genericRepository.ExistsAsync(x => x.Id == item.ItemModelId);
			if (!isItemModelAvailable)
				throw new ArgumentException("The specified item could not be found.");
			if (!(item.Quantity > 0))
				throw new ArgumentException("Quantity must be greater than zero.");
			var userItemModelQuantity = _mapper.Map<UserItem>(item);
			userItemModelQuantity.UserId = userId;
			var isItemQuantityAvailable = await userItemRepository.UserItemQuantityAvailableAsync(userItemModelQuantity);
			if (!isItemQuantityAvailable)
				throw new ArgumentException("The return requested quantity exceeds the available stock.");
			var itemModel = _mapper.Map<AddUserItemQuantityRequestModel>(item);
			itemModel.ItemRequestId = response.Id;
			var itemModelData = _mapper.Map<ItemReturnRequestItemModel>(itemModel);
			await repository.AddItemReturnRequestItemModelAsync(itemModelData);
		}
		return response;
	}

	public async Task<UserItemRequestResponseModel> UpdateItemReturnRequestAsync(
		int id,
		UpdateUserItemWithQuantityRequestModel itemRequest,
		int userId
	)
	{
		var user = await commonService.GetUser();
		if (!user.IsAdmin && (itemRequest.StatusId == (int)UserItemReturnStatus.Rejected || itemRequest.StatusId == (int)UserItemReturnStatus.Approved))
			throw new ArgumentException("Developer cannot Approve/Reject Item request.");
		if (user.IsAdmin && itemRequest.StatusId == (int)UserItemReturnStatus.Cancelled)
			throw new ArgumentException("Admin cannot Cancel Item request.");
		if (user.IsAdmin && (itemRequest.StatusId == (int)UserItemReturnStatus.Draft || itemRequest.StatusId == (int)UserItemReturnStatus.Pending))
			throw new ArgumentException("Admin cannot create a Draft/Pending Item request.");
		var isStatusExists = await IsStatusExists(itemRequest.StatusId);
		if (!isStatusExists)
			throw new ArgumentException("Status does not exists");
		var itemRequestData = await repository.GetItemReturnRequestByIdAsync(id);
		itemRequestData.Comment = itemRequest.Comment;
		var statusFromName = (UserItemReturnStatus)itemRequestData.StatusId;
		var statusToName = (UserItemReturnStatus)itemRequest.StatusId;
		if (itemRequestData.StatusId == (int)UserItemReturnStatus.Draft || (itemRequestData.StatusId == (int)UserItemReturnStatus.Pending && itemRequest.StatusId != (int)UserItemReturnStatus.Draft))
			itemRequestData.StatusId = itemRequest.StatusId;
		else
			throw new ArgumentException("Changing status from " + statusFromName + " to " + statusToName + " is not allowed.");
		if (!(itemRequest.ItemModels.Count > 0))
			throw new ArgumentException("At least one item must be included in the request.");

		var data = await repository.UpdateItemReturnRequestAsync(itemRequestData);

		var response = _mapper.Map<UserItemRequestResponseModel>(data);

		response.ItemModels = itemRequest.ItemModels;

		foreach (var item in itemRequest.ItemModels)
		{
			var userItemModelQuantity = _mapper.Map<UserItem>(item);
			userItemModelQuantity.UserId = userId;
			var isItemAvailable = await userItemRepository.UserItemQuantityAvailableAsync(userItemModelQuantity);
			if (!isItemAvailable)
				throw new ArgumentException("The requested quantity exceeds the available stock for Item Id." + item.ItemModelId);
			var itemModel = _mapper.Map<AddUserItemQuantityRequestModel>(item);
			itemModel.ItemRequestId = response.Id;
			var itemModelData = _mapper.Map<ItemReturnRequestItemModel>(itemModel);
			await repository.AddItemReturnRequestItemModelAsync(itemModelData);

			if (itemRequestData.StatusId == (int)UserItemReturnStatus.Approved)
			{
				var userItemQuantity = _mapper.Map<UserItem>(itemModel);
				userItemQuantity.UserId = userId;
				await userItemRepository.DeductUserItemQuantityAsync(userItemQuantity);
				var itemModelQuantity = _mapper.Map<ItemModel>(itemModel);
				await itemModelRepository.AddItemModelQuantityAsync(itemModelQuantity);
			}
		}
		return response;
	}

	public async Task<bool> IsStatusExists(int id)
	{
		var response = await returnRequestGenericRepository.ExistsAsync(x => x.Id == id);
		return response;
	}

	public async Task<string> GenerateRequestNumberAsync()
	{
		var now = DateTime.Now;
		string month = now.ToString("MMM");
		string year = now.Year.ToString();
		string prefix = $"RTN-{month}-{year}-";

		var searchParams = new UserItemRequestSearchParams();
		var existingRequestNumbers = await repository.GetAllItemReturnRequestsAsync(searchParams);

		int maxSequence = 0;

		foreach (var request in existingRequestNumbers.Records)
		{
			string[] parts = request.RequestNumber.Split('-');
			if (parts.Length == 4 && int.TryParse(parts[3], out int sequence))
			{
				if (sequence > maxSequence)
					maxSequence = sequence;
			}
		}

		int nextSequence = maxSequence + 1;
		var requestNumber = $"{prefix}{nextSequence}";
		return requestNumber;
	}
}