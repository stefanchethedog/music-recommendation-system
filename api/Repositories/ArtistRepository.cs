using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;

namespace repositories;

public interface IArtistRepository {
    Task<IEnumerable<Artist>> FindAll();
    Task<Artist?> FindOne(string id);
    Task<Artist> Create(Artist user);
    Task<Artist?> Delete(string id);
    Task<Artist?> Update(string id, string name);
}

public class ArtistRepository : IArtistRepository
{
    private readonly IDriver _driver;
    public ArtistRepository(IDriver driver) {
        _driver = driver;
    }
    public async Task<Artist> Create(Artist user)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans => {
            var cursor = await trans.RunAsync(@"
                CREATE (a:Artist {id: $id, name: $name}) 
                RETURN a {.id, .name};
            ", new { id = user.Id, name = user.Name});
            return await cursor.SingleAsync(rec => rec.AsObject<Artist>());
        });
    }

    public async Task<Artist?> Delete(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans => {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Artist {id: $id}) 
                WITH a, a.id AS id, a.name AS name 
                DETACH DELETE a RETURN id, name;
            ", new { id });
            if(await cursor.FetchAsync()) {
                return cursor.Current.AsObject<Artist>();
            }
            return null;
        });
    }

    public async Task<IEnumerable<Artist>> FindAll()
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Artist) 
                RETURN a { .id, .name };
            ");
            return await cursor.ToListAsync(rec => {
                return rec.AsObject<Artist>();
            });
        });
    }

    public async Task<Artist?> FindOne(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Artist) WHERE a.id = $id 
                RETURN a {.id, .name}
            ", new { id });
            if(await cursor.FetchAsync()){
                return cursor.Current.AsObject<Artist>();
            }
            else { return null; }
        });
    }

    public async Task<Artist?> Update(string id, string name)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                OPTIONAL MATCH (a:Artist {id: $id}) 
                WITH a WHERE a IS NOT NULL 
                SET a.name = $name RETURN a {.id, .name}
            ", new { id, name});
            if(await cursor.FetchAsync()){
                return cursor.Current.AsObject<Artist>();
            }
            else { return null; }
        });
    }

}