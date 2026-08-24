using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class Unit
{
    public int Id { get; set; }
    public string UnitName { get; set; } = "Unknown Unit";
    public string Sector { get; set; } = "General";

    [JsonIgnore]
    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
