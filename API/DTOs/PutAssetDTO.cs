

using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class PutAssetDTO
{
    [Required]
    public int UnitId { get; set; }

    [Required]
    public required string AssetSerial { get; set; } = string.Empty;
    [Required]
    public required string Type { get; set; } = "GenericAsset";
}
