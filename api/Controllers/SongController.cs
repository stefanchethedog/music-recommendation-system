using Microsoft.AspNetCore.Mvc;
using models;
using repositories;

namespace MusicRecommendationEngineAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SongsController : ControllerBase
{
    private readonly ISongRepository _repo;

    public SongsController(ISongRepository songRepository)
    {
        _repo = songRepository;
    }

    [HttpGet]
    public Task<IEnumerable<Song>> Get()
    {
        return _repo.FindAll();
    }
    [HttpGet]
    [Route("/findOne/:id")]
    public Task<Song> FindOne([FromQuery]String id)
    {
        return _repo.FindOne(id);
    }

    [HttpPost]
    public Task<Song> Create([FromBody] Song song){
        return _repo.Create(song);
    }

    [HttpDelete]
    public Task<Song> Delete([FromBody] String id){
        return _repo.Delete(id);
    }

    [HttpPut]
    public Task<Song> Update([FromQuery] String id, [FromBody] Song song)
    {
        return _repo.Update(id,song);
    }
}
