using Microsoft.AspNetCore.Mvc;
using models;
using repositories;

using UpdateAlbum = models.CreateAlbum;

namespace MusicRecommendationEngineAPI.Controllers;

[ApiController]
[Route("albums")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumRepository _repo;

    public AlbumsController(IAlbumRepository albumRepository)
    {
        _repo = albumRepository;
    }

    [HttpGet]
    public Task<IEnumerable<Album>> Get()
    {
        return _repo.FindAll();
    }
    [HttpGet(":id")]
    public async Task<ActionResult<Album?>> GetOne(string id) {
        var album = await _repo.FindOne(id);
        if(album == null) {
            return NotFound("Album with given id was not found");
        }
        return Ok(album);
    }
    [HttpPost]
    public async Task<ActionResult<Album>> Create([FromBody]CreateAlbum albumInfo) {
        string id = Guid.NewGuid().ToString();
        var album = new Album(id, albumInfo.Name);
        var result = await _repo.Create(album);
        if(result != null) { return Ok(result);}
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
    [HttpDelete(":id")]
    public async Task<ActionResult<Album?>> Delete(string id) {
        var result = await _repo.Delete(id);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "Album with given id was not found"
        });
    }

    [HttpPatch(":id")]
    public async Task<ActionResult<Album?>> Update(string id, [FromBody]UpdateAlbum albumInfo) {
        var result = await _repo.Update(id, albumInfo.Name);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "Album with given id was not found"
        });
    }
}
