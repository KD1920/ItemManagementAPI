using ItemManagement.Data;
using ItemManagement.Domain.Interface;

namespace ItemManagement.Repository;

public class OpenRepository(IGenericRepository<ItemType> itemTypeGenericRepository, IGenericRepository<ItemModel> itemModelGenericRepository) : IOpenRepository
{
	public async Task<List<ItemType>> GetAllItemTypeOptions()
	{
		var results = await itemTypeGenericRepository.GetAllAsync();
		return results;
	}
	public async Task<List<ItemModel>> GetAllItemModelOptions()
	{
		var results = await itemModelGenericRepository.GetAllAsync();
		return results;
	}
}
