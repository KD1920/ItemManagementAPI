using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Services;

public class ItemModelService(
	IItemModelRepository repository,
	IItemTypeRepository itemTypeRepository,
	IGenericRepository<ItemType> genericRepository,
	IGenericRepository<UserItem> userItemGenericRepository,
	IGenericRepository<ItemModel> itemModelGenericRepository,
	IMapper _mapper
) : IItemModelService
{
	public async Task<RecordTotalRecordResponseHelper<ItemModelResponseModel>> GetAllItemModelsAsync(ItemModelSearchParams searchParams)
	{
		var isSearchParamsValid = searchParams.PageSize > 0 && searchParams.Page > 0;
		if (!isSearchParamsValid)
			throw new ArgumentException("Please enter valid PageSize or Page.");
		if (searchParams.ItemTypeId != 0)
		{
			var isItemTypeIdExists = await genericRepository.ExistsAsync(x => x.Id == searchParams.ItemTypeId);
			if (!isItemTypeIdExists)
				throw new ArgumentException("ItemTypeId does not exists");
		}
		var data = await repository.GetAllItemModelsAsync(searchParams);
		var itemModelAutoMapper = new List<ItemModelResponseModel>();
		foreach (var item in data.Records)
		{
			itemModelAutoMapper.Add(_mapper.Map<ItemModelResponseModel>(item));
		}
		 var response = new RecordTotalRecordResponseHelper<ItemModelResponseModel>();
        response.Records = itemModelAutoMapper;
        response.TotalRecords = data.TotalRecords;
		return response;
	}

	public async Task<ItemModelResponseModel> GetItemModelByIdAsync(int id)
	{
		var data = await repository.GetItemModelByIdAsync(id);
		if (data is null)
			throw new KeyNotFoundException();
		var response = _mapper.Map<ItemModelResponseModel>(data);
		return response;
	}
	public async Task<ItemModelResponseModel> AddItemModelAsync(AddItemModelRequestModel itemModel)
	{
		var isItemModelExists = await itemModelGenericRepository.ExistsAsync(x => x.Name.ToLower() == itemModel.Name.ToLower() && x.ItemTypeId == itemModel.ItemTypeId);
		if (isItemModelExists)
			throw new ArgumentException("Item Model with this name already exists for same Item Type please provide an unique name.");
		var isItemTypeAvailable = await itemTypeRepository.GetItemTypeByIdAsync(itemModel.ItemTypeId);
		if (isItemTypeAvailable == null)
			throw new ArgumentException("Item Type is not available.");
		var itemModelAutoMapper = _mapper.Map<ItemModel>(itemModel);
		var data = await repository.AddItemModelAsync(itemModelAutoMapper);
		var response = _mapper.Map<ItemModelResponseModel>(data);
		return response;
	}

	public async Task<ItemModelResponseModel> UpdateItemModelAsync(int id, AddItemModelRequestModel itemModel)
	{
		var isItemTypeAvailable = await itemTypeRepository.GetItemTypeByIdAsync(itemModel.ItemTypeId);
		if (isItemTypeAvailable == null)
			throw new ArgumentException("Item Type is not available.");
		var itemModelData = await repository.GetItemModelByIdAsync(id);
		if (itemModelData is null)
			throw new KeyNotFoundException();
		var isItemModelExists = await itemModelGenericRepository.ExistsAsync(x => x.Id != id && x.Name.ToLower() == itemModel.Name.ToLower() && x.ItemTypeId == itemModel.ItemTypeId);
		if (isItemModelExists)
			throw new ArgumentException("Item Model with this name already exists for same Item Type please provide an unique name.");
		var data = new ItemModel();
		var response = new ItemModelResponseModel();
		if (itemModelData != null)
		{
			_mapper.Map(itemModel, itemModelData);
			data = await repository.UpdateItemModelAsync(itemModelData);
			response = _mapper.Map<ItemModelResponseModel>(data);
		}
		return response;
	}

	public async Task<ItemModelResponseModel> DeleteItemModelAsync(int id)
	{
		var itemModelData = await userItemGenericRepository.ExistsAsync(x => x.ItemModelId == id);

		if (itemModelData)
			throw new ArgumentException("Item Model is already in use you can't delete it.");

		var data = await repository.DeleteItemModelAsync(id);
		if (data is null)
			throw new KeyNotFoundException();
		var response = _mapper.Map<ItemModelResponseModel>(data);
		return response;
	}
}
