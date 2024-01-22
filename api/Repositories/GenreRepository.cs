using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;

namespace repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> FindAll();
    Task<Genre?> FindOne(string id);
    Task<Genre> Create(Genre genre);
    Task<Genre?> Delete(string id);
    Task<Genre?> Update(string id, string name);
}

public class GenreRepository : IGenreRepository
{
    private readonly IDriver _driver;
    public GenreRepository(IDriver driver)
    {
        _driver = driver;
    }
    public async Task<Genre> Create(Genre genre)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans =>
        {
            var cursor = await trans.RunAsync(@"
                CREATE (g:Genre {id: $id, name: $name}) 
                RETURN g {.id, .name};
            ", new { id = genre.Id, name = genre.Name });
            return await cursor.SingleAsync(rec => rec.AsObject<Genre>());
        });
    }

    public async Task<Genre?> Delete(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (g:Genre {id: $id}) 
                WITH g, g.id AS id, g.name AS name 
                DETACH DELETE g RETURN id, name;
            ", new { id });
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<Genre>();
            }
            return null;
        });
    }

    public async Task<IEnumerable<Genre>> FindAll()
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (g:Genre) 
                RETURN g { .id, .name };
            ");
            return await cursor.ToListAsync(rec =>
            {
                return rec.AsObject<Genre>();
            });
        });
    }

    public async Task<Genre?> FindOne(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (g:Genre) WHERE g.id = $id 
                RETURN g {.id, .name}
            ", new { id });
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<Genre>();
            }
            else { return null; }
        });
    }

    public async Task<Genre?> Update(string id, string name)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                OPTIONAL MATCH (g:Genre {id: $id}) 
                WITH g WHERE g IS NOT NULL 
                SET g.name = $name RETURN g {.id, .name}
            ", new { id, name });
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<Genre>();
            }
            else { return null; }
        });
    }

}