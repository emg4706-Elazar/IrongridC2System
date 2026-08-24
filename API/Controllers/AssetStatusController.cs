using API.DTOs;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/assets-status/")]
public class AssetStatusController : ControllerBase
{
    private readonly IIrongridRepository _repository;

    public AssetStatusController(IIrongridRepository repository)
    {
        _repository = repository;
    }


    // Get all Asset combined with there live status
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAssetStatusDetailedDTO>>>
        GetAllAssetsDatailes()
    {
        var assets = await _repository
            .GetAllAssetsDtailesAsync();

        return Ok(assets);
    }


    // Get all assets by status
    [HttpGet("status")]
    public async Task<IActionResult> GetAssetsByStatus(string status)
    {
        List<string> AllowedStatuses = new() { "warning", "stable" };

        if (!AllowedStatuses.Contains(status.Trim().ToLower()))
        {
            return BadRequest("Allowd Values: 'Warning', 'Stable'.");
        }

        var assets = await _repository
            .GetAssetsByStatusAsync(status);

        return Ok(assets);
    }
}
