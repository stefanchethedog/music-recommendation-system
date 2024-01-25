using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using models;
using repositories;
using views;
using UpdateUser = models.CreateUser;

namespace MusicRecommendationEngineAPI.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;

    public UsersController(IUserRepository userRepository)
    {
        _repo = userRepository;
    }

    [HttpGet]
    public Task<IEnumerable<User>> Get()
    {
        return _repo.FindAll();
    }
    [HttpGet(":id")]
    public async Task<ActionResult<User?>> GetOne(string id)
    {
        var user = await _repo.FindOne(id);
        if (user == null)
        {
            return NotFound("User with given id was not found");
        }
        return Ok(user);
    }

    [HttpGet]
    [Route("getLikedSongs")]
    public async Task<ActionResult<List<SongView>?>> getLikedSongs(string id)
    {
        var user = await _repo.GetLikedSongs(id);
        if(user == null)
        {
            return NotFound("User with given id was not found");
        }
        return Ok(user);
    }


    [HttpGet]
    [Route("getUserByUsername")]
    public async Task<ActionResult<User?>> GetUserByUsername(string username)
    {
        var user = await _repo.FindUserByUsername(username);
        if(user == null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUser userInfo)
    {
        string id = Guid.NewGuid().ToString();
        var user = new User(id, userInfo.Username);
        var result = await _repo.Create(user);
        if (result != null) { return Ok(result); }
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
    [HttpDelete(":id")]
    public async Task<ActionResult<User?>> Delete(string id)
    {
        var result = await _repo.Delete(id);
        if (result != null) { return Ok(result); }
        return NotFound(new
        {
            error = "User with given id was not found"
        });
    }

    [HttpPatch(":id")]
    public async Task<ActionResult<User?>> Update(string id, [FromBody] UpdateUser userInfo)
    {
        var result = await _repo.Update(id, userInfo.Username);
        if (result != null) { return Ok(result); }
        return NotFound(new
        {
            error = "User with given id was not found"
        });
    }

    [HttpPost]
    [Route("AddUserLikesSong")]
    public async Task<ActionResult<User?>> AddUserLikesSong([FromQuery] string id,[FromQuery] string songName)
    {
        var result = await _repo.AddLikesSong(id, songName);
        if(result != null) { return Ok(result); }
        return NotFound(new{
            error = "Error"
        });
    }
    [HttpPost]
    [Route("Follow")]
    public async Task<ActionResult<User?>> Follow([FromQuery] string id,[FromQuery] string username)
    {
        var result = await _repo.FollowUser(id, username);
        if(result != null) { return Ok(result); }
        return NotFound(new{
            error = "Error"
        });
    }

    [HttpPost]
    [Route("subscribe")]
    public async Task<ActionResult<User?>> Subscribe([FromQuery] string id,[FromQuery] string name)
    {
        var result = await _repo.Subscribe(id, name);
        if(result != null) { return Ok(result); }
        return NotFound(new{
            error = "Error"
        });
    }
    
    [HttpGet]
    [Route("RecommendSongsByLikedSongs")]
    public async Task<ActionResult<List<SongView>?>> RecommendSongsByLikedSongs([FromQuery]string id)
    {
        var result = await _repo.FindOtherUsersSongs(id);

        return result;
    }
    [HttpGet]
    [Route("GetRecommendedSongsByFollowingUsers")]
    public async Task<ActionResult<List<SongView>?>> GetRecommendedSongsByFollowingUsers([FromQuery] string id)
    {
        return await _repo.FindSongsByTheFollowedUsers(id);
    }
}
