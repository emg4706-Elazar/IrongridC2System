


namespace API.DTOs;

public class GetAssetStatusDetailedDTO
{
    public int AssetId { get; set; }
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string Type { get; set; } = null!;
    public string RawValue { get; set; } = string.Empty;
    public string ProcessedStatus { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime LastUpdate { get; set; }
}
