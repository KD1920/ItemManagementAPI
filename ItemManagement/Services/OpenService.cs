using AutoMapper;
using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;

namespace ItemManagement.Services;

public class OpenService(IOpenRepository openRepository, IMapper _mapper) : IOpenService
{
	public async Task<List<CommonOptionResponseHelper>> GetAllItemTypeOptions()
	{
		var results = await openRepository.GetAllItemTypeOptions();
		var mapper = new List<CommonOptionResponseHelper>();
		foreach (var item in results)
		{
			item.Name = item.Name.ToString();
			mapper.Add(_mapper.Map<CommonOptionResponseHelper>(item));
		}
		return mapper;
	}

	public async Task<List<CommonOptionResponseHelper>> GetAllItemModelOptions()
	{
		var results = await openRepository.GetAllItemModelOptions();
		var mapper = new List<CommonOptionResponseHelper>();
		foreach (var item in results)
		{
			item.Name = item.Name.ToString();
			mapper.Add(_mapper.Map<CommonOptionResponseHelper>(item));
		}
		return mapper;
	}
}
