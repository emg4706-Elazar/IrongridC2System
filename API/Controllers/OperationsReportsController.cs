using API.Repositories;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;


namespace API.Controllers;

[ApiController]
[Route("/api/reports/")]
public class OperationsReportsController : ControllerBase
{
    private readonly IIrongridRepository _repository;
    public OperationsReportsController(IIrongridRepository repository)
    {
        _repository = repository;
    }


    // Get critical assets 
    [HttpGet("critical-assets")]
    public async Task<ActionResult<IEnumerable<GetCriticalAssetsDTO>>>
        GetCriticalAssets()
    {
        var assets = await _repository
            .GetCriticalAssetsAsync();

        return Ok(assets);
    }


    // Get unit's assets
    [HttpGet("unit/{unitId}/assets")]
    public async Task<ActionResult<IEnumerable<GetUnitAssetsDTO>>>
        GetUnitAssets(int unitId)
    {
        var assetStatus = await _repository
            .GetUnitAssetsAsync(unitId);

        if (assetStatus == null)
            return NotFound();

        return Ok(assetStatus);
    }
}
