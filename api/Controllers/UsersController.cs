using Microsoft.AspNetCore.Mvc;
using models;
using repositories;

namespace MusicRecommendationEngineAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
}
