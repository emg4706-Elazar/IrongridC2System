using API.Models;
using API.Data;
using API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class IrongridRepository : IIrongridRepository
{
    private readonly AppDbContext _context;
    public IrongridRepository(AppDbContext context)
    {
        _context = context;
    }


    // Get Asset by id
    public async Task<GetAssetDTO?> GetAssetByIdAsync(int id)
    {
        var asset = await _context.Assets.FindAsync(id);

        if (asset == null)
            return null;

        return new GetAssetDTO()
        {
            Id = asset.Id,
            UnitId = asset.UnitId,
            AssetSerial = asset.AssetSerial,
            Type = asset.Type
        };
    }


    // Create new unit
    public async Task<bool> CreateUnitAsync(PostUnitDTO unit)
    {
        Unit created = new Unit()
        {
            UnitName = unit.UnitName,
            Sector = unit.Sector
        };

        _context.Units.Add(created);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return false;
        }
    }


    // Update existed asset
    public async Task<Asset?> UpdateAssetAsync(PutAssetDTO updated, int id)
    {
        var existing = await _context.Assets.FindAsync(id);

        if (existing == null)
            return null;

        existing.UnitId = updated.UnitId;
        existing.AssetSerial = updated.AssetSerial;
        existing.Type = updated.Type;

        await _context.SaveChangesAsync();

        return existing;
    }


    // Delete Asset
    public async Task<bool> DeleteAssetAsync(int id)
    {
        var existing = await _context.Assets.FindAsync(id);

        if (existing == null)
            return false;

        _context.Assets.Remove(existing);
        await _context.SaveChangesAsync();

        return true;
    }


    // Get all Asset combined with there live status
    public async Task<IEnumerable<GetAssetStatusDetailedDTO>>
        GetAllAssetsDtailesAsync()
    {
        return await _context.AssetLiveStatus
            .Select(s => new GetAssetStatusDetailedDTO()
            {
                AssetId = s.Asset.Id,
                UnitId = s.Asset.UnitId,
                AssetSerial = s.Asset.AssetSerial,
                Type = s.Asset.Type,
                RawValue = s.RawValue,
                ProcessedStatus = s.ProcessedStatus,
                IsVerified = s.IsVerified,
                LastUpdate = s.LastUpdate
            }).ToListAsync();
    }


    // Get Assets by status
    public async Task<IEnumerable<GetAssetDTO>>
        GetAssetsByStatusAsync(string status)
    {
        return await _context.Assets
            .Where(s => s.AssetStatus.ProcessedStatus.ToLower() == status)
            .Select(s => new GetAssetDTO
            {
                Id = s.Id,
                UnitId = s.UnitId,
                AssetSerial = s.AssetSerial,
                Type = s.Type
            }).ToListAsync();
    }

    // Get critical assets
    public async Task<IEnumerable<GetCriticalAssetsDTO>>
        GetCriticalAssetsAsync()
    {
        return await _context.Assets
            .Where(s => 
            s.AssetStatus.ProcessedStatus == "Warning" ||
            !s.AssetStatus.IsVerified)
            .Select(s => new GetCriticalAssetsDTO
            {
                AssetId = s.Id,
                AssetSerial = s.AssetSerial,
                AssetType = s.Type,
                UnitName = s.Unit.UnitName,
                Sector = s.Unit.Sector,
                ProcessedStatus = s.AssetStatus.ProcessedStatus,
                IsVerified = s.AssetStatus.IsVerified,
                LastUpdae = s.AssetStatus.LastUpdate
            }).ToListAsync();
    }


    // Get unit's assets
    public async Task<IEnumerable<GetUnitAssetsDTO>?> GetUnitAssetsAsync(int unitId)
    {
        var existUnit = await _context.Units.FindAsync(unitId);
        if (existUnit == null)
            return null;

        return await _context.AssetLiveStatus
            .Where(s => s.Asset.UnitId == unitId)
            .Select(s => new GetUnitAssetsDTO
            {
                AssetId = s.AssetId,
                AssetSerial = s.Asset.AssetSerial,
                AssetType = s.AssetType,
                ProcessedStatus = s.ProcessedStatus,
                IsVerified = s.IsVerified,
                LastUpdate = s.LastUpdate
            }).ToListAsync();
    }
}
