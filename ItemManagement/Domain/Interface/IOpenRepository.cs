using ItemManagement.Data;

namespace ItemManagement.Domain.Interface;

public interface IOpenRepository
{
	Task<List<ItemType>> GetAllItemTypeOptions();
	Task<List<ItemModel>> GetAllItemModelOptions();
}
