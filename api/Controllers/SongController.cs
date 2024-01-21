using Microsoft.AspNetCore.Mvc;
using models;
using views;
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
    [Route("GetAll")]
    public Task<IEnumerable<SongView>> Get()
    {
        return _repo.FindAll();
    }
    [HttpGet]
    [Route("GetOne")]
    public Task<SongView?> FindOne([FromQuery]String id)
    {
        return _repo.FindOne(id);
    }

    [HttpPost]
    [Route("Create")]
    public Task<SongView?> Create([FromBody] CreateSong song){
        return _repo.Create(song);
    }

    [HttpDelete]
    [Route("DeleteSong")]
    public Task<SongView?> Delete([FromQuery] String id){
        return _repo.Delete(id);
    }

    [HttpDelete]
    [Route("RemoveGenre")]
    public Task<SongView?> RemoveGenre([FromQuery] string id, [FromQuery] string genre)
    {
        return _repo.RemoveGenre( id, genre);
    }
    
    [HttpPatch]
    [Route("Update")]
    public Task<SongView?> Update([FromQuery] String id, [FromBody] SongView song)
    {
        return _repo.Update(id, song.Name, song.Author, song.Album, song.Genres);
        //string id, string newName, string newAuthor, string newAlbum
    }

    [HttpPatch]
    [Route("UpdateArtist")]
    public Task<SongView?> UpdateArtist([FromQuery] string id, [FromQuery] string newArtist)
    {
        return _repo.UpdateArtist(id,newArtist);
    } 
    [HttpPatch]
    [Route("UpdateAlbum")]
    public Task<SongView?> UpdateAlbum([FromQuery] string id, [FromQuery] string? newAlbum)
    {
        return _repo.UpdateAlbum(id,newAlbum);
    } 

    [HttpPatch]
    [Route("UpdateName")]
    public Task<SongView?> UpdateName([FromQuery] string id, [FromQuery] string newName)
    {
        return _repo.UpdateName(id,newName);
    } 

    [HttpPatch]
    [Route("UpdateGenres")]
    public Task<SongView?> UpdateGenres([FromQuery] string id, [FromBody] List<string> newGenres)
    {
        return _repo.UpdateGenres(id, newGenres);
    }

    [HttpPatch]
    [Route("AddGenre")]
    public Task<SongView?> AddGenre([FromQuery] string id, [FromQuery] string genre)
    {
        return _repo.AddGenre( id, genre );
    }

    
}
