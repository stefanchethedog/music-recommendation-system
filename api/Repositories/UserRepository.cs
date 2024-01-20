using models;
using Neo4j.Driver;

namespace repositories;

public interface IUserRepository {
    Task<IEnumerable<User>> FindAll();
    Task<User> FindOne(int id);
    Task<User> Create(User user);
    Task<User> Delete(int id);
    Task<User> Update(int id, User user);
}

public class UserRepository : IUserRepository
{
    private readonly IDriver _driver;
    public UserRepository(IDriver driver) {
        _driver = driver;
    }
    public Task<User> Create(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> FindAll()
    {
        var session = _driver.AsyncSession();
        return (IEnumerable<User>)await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (u:User) RETURN u { .id, .username } as u
            ");
            return await cursor.ToListAsync(rec => {
                return new User(
                    rec["id"].As<int>(),
                    rec["username"].As<string>()
                );});
        });
    }

    public Task<User> FindOne(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Update(int id, User user)
    {
        throw new NotImplementedException();
    }
}