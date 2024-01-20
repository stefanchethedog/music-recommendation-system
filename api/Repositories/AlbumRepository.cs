using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;

namespace repositories;

public interface IAlbumRepository {
    Task<IEnumerable<Album>> FindAll();
    Task<Album?> FindOne(string id);
    Task<Album> Create(Album user);
    Task<Album?> Delete(string id);
    Task<Album?> Update(string id, string name);
}

public class AlbumRepository : IAlbumRepository
{
    private readonly IDriver _driver;
    public AlbumRepository(IDriver driver) {
        _driver = driver;
    }
    public async Task<Album> Create(Album user)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans => {
            var cursor = await trans.RunAsync(@"
                CREATE (a:Album {id: $id, name: $name}) 
                RETURN a {.id, .name};
            ", new { id = user.Id, name = user.Name});
            return await cursor.SingleAsync(rec => rec.AsObject<Album>());
        });
    }

    public async Task<Album?> Delete(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans => {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Album {id: $id}) 
                WITH a, a.id AS id, a.name AS name 
                DETACH DELETE a RETURN id, name;
            ", new { id });
            if(await cursor.FetchAsync()) {
                return cursor.Current.AsObject<Album>();
            }
            return null;
        });
    }

    public async Task<IEnumerable<Album>> FindAll()
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Album) 
                RETURN a { .id, .name };
            ");
            return await cursor.ToListAsync(rec => {
                return rec.AsObject<Album>();
            });
        });
    }

    public async Task<Album?> FindOne(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Album) WHERE a.id = $id 
                RETURN a {.id, .name}
            ", new { id });
            if(await cursor.FetchAsync()){
                return cursor.Current.AsObject<Album>();
            }
            else { return null; }
        });
    }

    public async Task<Album?> Update(string id, string name)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                OPTIONAL MATCH (a:Album {id: $id}) 
                WITH a WHERE a IS NOT NULL 
                SET a.name = $name RETURN a {.id, .name}
            ", new { id, name});
            if(await cursor.FetchAsync()){
                return cursor.Current.AsObject<Album>();
            }
            else { return null; }
        });
    }

}