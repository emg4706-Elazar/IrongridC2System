using API.DTOs;
using API.Models;

namespace API.Repositories;

public interface IIrongridRepository
{
    Task<GetAssetDTO?> GetAssetByIdAsync(int id);
    Task<bool> CreateUnitAsync(PostUnitDTO unit);
    Task<Asset?> UpdateAssetAsync(PutAssetDTO asset, int id);
    Task<bool> DeleteAssetAsync(int id);
    Task<IEnumerable<GetAssetStatusDetailedDTO>> GetAllAssetsDtailesAsync();
    Task<IEnumerable<GetAssetDTO>> GetAssetsByStatusAsync(string status);
    Task<IEnumerable<GetCriticalAssetsDTO>> GetCriticalAssetsAsync();
    Task<IEnumerable<GetUnitAssetsDTO>?> GetUnitAssetsAsync(int unitId);
}
