using System.ComponentModel.DataAnnotations;


namespace API.DTOs;

public class PostUnitDTO
{
    [Required]
    public required string UnitName { get; set; } = "Unknown Unit";

    [Required]
    public required string Sector { get; set; } = "General";
}
