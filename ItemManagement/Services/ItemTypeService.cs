using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Services;

public class ItemTypeService(IItemTypeRepository repository, IGenericRepository<ItemModel> genericRepository, IGenericRepository<ItemType> itemTypeGenericRepository, IMapper _mapper) : IItemTypeService
{
    public async Task<RecordTotalRecordResponseHelper<ItemTypeResponseModel>> GetAllItemTypesAsync(CommonRequestHelper commonRequestHelper)
    {
        var isSearchParamsValid = commonRequestHelper.PageSize > 0 && commonRequestHelper.Page > 0;
        if (!isSearchParamsValid)
            throw new ArgumentException("Please enter valid PageSize or Page.");
        var data = await repository.GetAllItemTypesAsync(commonRequestHelper);
        var itemTypeAutoMapper = new List<ItemTypeResponseModel>();
        foreach (var item in data.Records)
        {
            itemTypeAutoMapper.Add(_mapper.Map<ItemTypeResponseModel>(item));
        }
        var response = new RecordTotalRecordResponseHelper<ItemTypeResponseModel>();
        response.Records = itemTypeAutoMapper;
        response.TotalRecords = data.TotalRecords;
        return response;
    }

    public async Task<ItemTypeResponseModel> GetItemTypeByIdAsync(int id)
    {
        var data = await repository.GetItemTypeByIdAsync(id);
        if (data is null)
            throw new KeyNotFoundException();
        var response = _mapper.Map<ItemTypeResponseModel>(data);
        return response;
    }
    public async Task<ItemTypeResponseModel> AddItemTypeAsync(AddItemTypeRequestModel itemType)
    {
        var isItemTypeExists = await itemTypeGenericRepository.ExistsAsync(x => x.Name.ToLower() == itemType.Name.ToLower());
        if (isItemTypeExists)
            throw new ArgumentException("Item Type with this name already exists please provide an unique name.");
        var itemTypeAutoMapper = _mapper.Map<ItemType>(itemType);
        var data = await repository.AddItemTypeAsync(itemTypeAutoMapper);
        var response = _mapper.Map<ItemTypeResponseModel>(data);
        return response;
    }

    public async Task<ItemTypeResponseModel> UpdateItemTypeAsync(int id, AddItemTypeRequestModel itemType)
    {
        var itemTypeData = await repository.GetItemTypeByIdAsync(id);
        if (itemTypeData is null)
            throw new KeyNotFoundException();
        var isItemTypeExists = await itemTypeGenericRepository.ExistsAsync(x => x.Name.ToLower() == itemType.Name.ToLower() && x.Id != id);
        if (isItemTypeExists)
            throw new ArgumentException("Item Type with this name already exists please provide an unique name.");
        var data = new ItemType();
        var response = new ItemTypeResponseModel();
        if (itemTypeData != null)
        {
            _mapper.Map(itemType, itemTypeData);
            data = await repository.UpdateItemTypeAsync(itemTypeData);
            response = _mapper.Map<ItemTypeResponseModel>(data);
        }
        return response;
    }

    public async Task<ItemTypeResponseModel> DeleteItemTypeAsync(int id)
    {
        var itemModelData = await genericRepository.ExistsAsync(x => x.ItemTypeId == id);

        if (itemModelData)
            throw new ArgumentException("Item Type is already in use you can't delete it.");

        var data = await repository.DeleteItemTypeAsync(id);
        if (data is null)
            throw new KeyNotFoundException();

        var response = _mapper.Map<ItemTypeResponseModel>(data);
        return response;
    }
}

