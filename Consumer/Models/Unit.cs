using System.ComponentModel.DataAnnotations;

namespace Consumer.Models;

public class Unit
{
    [Required]
    public int Id { get; set; }
    public string UnitName { get; set; } = "Unknown Unit";
    public string Sector { get; set; } = "General";
}
