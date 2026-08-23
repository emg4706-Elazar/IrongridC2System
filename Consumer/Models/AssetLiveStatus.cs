

using System.ComponentModel.DataAnnotations;

namespace Consumer.Models;

public class AssetLiveStatus
{
    public int AssetId { get; set; }
    public string AssetType { get; set; } = string.Empty;

    [Required]
    public string RawValue { get; set; } = string.Empty;
    public string ProcessedStatus { get; set; } = string.Empty;

    [Required]
    public bool IsVerified { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
}
