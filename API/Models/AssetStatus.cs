

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class AssetStatus
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

    [JsonIgnore]
    public Asset Asset { get; set; } = null!;
}
