using Microsoft.AspNetCore.Mvc;
using models;
using repositories;
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
    public async Task<ActionResult<User?>> GetOne(string id) {
        var user = await _repo.FindOne(id);
        if(user == null) {
            return NotFound("User with given id was not found");
        }
        return Ok(user);
    }
    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody]CreateUser userInfo) {
        string id = Guid.NewGuid().ToString();
        var user = new User(id, userInfo.Username);
        var result = await _repo.Create(user);
        if(result != null) { return Ok(result);}
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
    [HttpDelete(":id")]
    public async Task<ActionResult<User?>> Delete(string id) {
        var result = await _repo.Delete(id);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "User with given id was not found"
        });
    }

    [HttpPatch(":id")]
    public async Task<ActionResult<User?>> Update(string id, [FromBody]UpdateUser userInfo) {
        var result = await _repo.Update(id, userInfo.Username);
        if(result != null) { return Ok(result);}
        return NotFound(new {
            error = "User with given id was not found"
        });
    }
}
