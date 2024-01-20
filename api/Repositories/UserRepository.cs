using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;

namespace repositories;

public interface IUserRepository {
    Task<IEnumerable<User>> FindAll();
    Task<User?> FindOne(string id);
    Task<User> Create(User user);
    Task<User?> Delete(string id);
    Task<User?> Update(string id, string username);
}

public class UserRepository : IUserRepository
{
    private readonly IDriver _driver;
    public UserRepository(IDriver driver) {
        _driver = driver;
    }
    public async Task<User> Create(User user)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans => {
            var cursor = await trans.RunAsync(@"
                CREATE (u:User {id: $id, username: $username}) 
                RETURN u {.id, .username};
            ", new { id = user.Id, username = user.Username});
            return await cursor.SingleAsync(rec => rec.AsObject<User>());
        });
    }

    public async Task<User?> Delete(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans => {
            var cursor = await trans.RunAsync(@"
                MATCH (u:User {id: $id}) 
                WITH u, u.id AS id, u.username AS username 
                DETACH DELETE u RETURN id, username;
            ", new { id });
            if(await cursor.FetchAsync()) {
                return cursor.Current.AsObject<User>();
            }
            return null;
        });
    }

    public async Task<IEnumerable<User>> FindAll()
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (u:User) 
                RETURN u { .id, .username };
            ");
            return await cursor.ToListAsync(rec => {
                return rec.AsObject<User>();
            });
        });
    }

    public async Task<User?> FindOne(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (u:User) WHERE u.id = $id 
                RETURN u {.id, .username}
            ", new { id });
            if(await cursor.FetchAsync()){
                return cursor.Current.AsObject<User>();
            }
            else { return null; }
        });
    }

    public async Task<User?> Update(string id, string username)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                OPTIONAL MATCH (user:User {id: $id}) 
                WITH user WHERE user IS NOT NULL 
                SET user.username = $username RETURN user {.id, .username}
            ", new { id, username});
            if(await cursor.FetchAsync()){
                return cursor.Current.AsObject<User>();
            }
            else { return null; }
        });
    }

}