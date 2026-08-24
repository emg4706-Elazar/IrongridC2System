

namespace API.DTOs;

public class GetUnitSummaryDTO
{
    public int UnitId { get; set; }
    public string UnitName { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public int TotalAssets { get; set; }
    public int StableAssets { get; set; }
    public int WarningAssets { get; set; }
    public int UnverfiedAssets { get; set; }
}
