


namespace API.DTOs;

public class GetAssetDTO
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string Type { get; set; } = null!;
}
