using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.DTOs;



namespace API.Controllers;

[ApiController]
[Route("/api/assets/")]
public class AssetsController : ControllerBase
{
    private readonly IIrongridRepository _repository;

    public AssetsController(IIrongridRepository repository)
    {
        _repository = repository;
    }


    // Get Asset by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetById(int id)
    {
        var asset = await _repository
            .GetAssetByIdAsync(id);
        
        if (asset == null)
        {
            return NotFound();
        }

        return Ok(asset);
    }


    // Create new Unit
    [HttpPost("units")]
    public async Task<ActionResult> CreateUnit(PostUnitDTO unit)
    {
        bool success = await _repository.CreateUnitAsync(unit);

        return StatusCode(201);
    }


    // Update Asset
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(int id, PutAssetDTO asset)
    {
        var updated = await _repository.UpdateAssetAsync(asset, id);

        if (updated == null)
            return NotFound();

        return Ok(updated);
    }


    // Delete Existed Asset
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var success = await _repository.DeleteAssetAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
