


namespace API.DTOs;

public class GetCriticalAssetsDTO
{
    public int AssetId { get; set; }
    public string AssetSerial { get; set; } = null!;
    public string AssetType { get; set; } = null!;
    public string UnitName { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public string ProcessedStatus { get; set; } = null!;
    public bool IsVerified { get; set; }
    public DateTime LastUpdae { get; set; }
}
