

using System.Globalization;

namespace API.DTOs;

public class GetUnitAssetsDTO
{
    public int AssetId { get; set; }
    public string AssetSerial { get; set; } = null!;
    public string AssetType { get; set; } = null!;
    public string ProcessedStatus { get; set; } = null!;
    public bool IsVerified { get; set; }
    public DateTime LastUpdate { get; set; }
}
