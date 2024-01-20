using Microsoft.AspNetCore.Mvc;
using models;
using repositories;

using UpdateGenre = models.CreateGenre;

namespace MusicRecommendationEngineAPI.Controllers;

[ApiController]
[Route("genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreRepository _repo;

    public GenresController(IGenreRepository albumRepository)
    {
        _repo = albumRepository;
    }

    [HttpGet]
    public Task<IEnumerable<Genre>> Get()
    {
        return _repo.FindAll();
    }
    [HttpGet(":id")]
    public async Task<ActionResult<Genre?>> GetOne(string id) {
        var album = await _repo.FindOne(id);
        if(album == null) {
            return NotFound("Genre with given id was not found");
        }
        return Ok(album);
    }
    [HttpPost]
    public async Task<ActionResult<Genre>> Create([FromBody]CreateGenre albumInfo) {
        string id = Guid.NewGuid().ToString();
        var album = new Genre(id, albumInfo.Name);
        var result = await _repo.Create(album);
        if(result != null) { return Ok(result);}
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
    [HttpDelete(":id")]
    public async Task<ActionResult<Genre?>> Delete(string id) {
        var result = await _repo.Delete(id);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "Genre with given id was not found"
        });
    }

    [HttpPatch(":id")]
    public async Task<ActionResult<Genre?>> Update(string id, [FromBody]UpdateGenre albumInfo) {
        var result = await _repo.Update(id, albumInfo.Name);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "Genre with given id was not found"
        });
    }
}
