using ItemManagement.Common.Helpers;

namespace ItemManagement.Services.Interfaces;
public interface IOpenService
{
    Task<List<CommonOptionResponseHelper>> GetAllItemTypeOptions();
    Task<List<CommonOptionResponseHelper>> GetAllItemModelOptions();
}