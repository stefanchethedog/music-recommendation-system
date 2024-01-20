using Microsoft.AspNetCore.Mvc;
using models;
using repositories;

using UpdateArtist = models.CreateArtist;

namespace MusicRecommendationEngineAPI.Controllers;

[ApiController]
[Route("artists")]
public class ArtistsController : ControllerBase
{
    private readonly IArtistRepository _repo;

    public ArtistsController(IArtistRepository artistRepository)
    {
        _repo = artistRepository;
    }

    [HttpGet]
    public Task<IEnumerable<Artist>> Get()
    {
        return _repo.FindAll();
    }
    [HttpGet(":id")]
    public async Task<ActionResult<Artist?>> GetOne(string id) {
        var artist = await _repo.FindOne(id);
        if(artist == null) {
            return NotFound("Artist with given id was not found");
        }
        return Ok(artist);
    }
    [HttpPost]
    public async Task<ActionResult<Artist>> Create([FromBody]CreateArtist artistInfo) {
        string id = Guid.NewGuid().ToString();
        var artist = new Artist(id, artistInfo.Name);
        var result = await _repo.Create(artist);
        if(result != null) { return Ok(result);}
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
    [HttpDelete(":id")]
    public async Task<ActionResult<Artist?>> Delete(string id) {
        var result = await _repo.Delete(id);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "Artist with given id was not found"
        });
    }

    [HttpPatch(":id")]
    public async Task<ActionResult<Artist?>> Update(string id, [FromBody]UpdateArtist artistInfo) {
        var result = await _repo.Update(id, artistInfo.Name);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "Artist with given id was not found"
        });
    }
}
