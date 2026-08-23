using System.ComponentModel.DataAnnotations;

namespace Consumer.Models;

public class Asset
{
    [Required]
    public int id { get; set; }

    public int UnitId { get; set; }

    [Required]
    public string AssetSerial { get; set; } = string.Empty;

    public string AssetType { get; set; } = "GenericAsset";
}
