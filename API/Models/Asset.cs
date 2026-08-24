using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class Asset
{
    [Required]
    public int Id { get; set; }

    public int UnitId { get; set; }

    [Required]
    public string AssetSerial { get; set; } = string.Empty;

    public string Type { get; set; } = "GenericAsset";

    [JsonIgnore]
    public Unit Unit { get; set; } = null!;

    [JsonIgnore]
    public AssetStatus AssetStatus { get; set; } = null!;
}
