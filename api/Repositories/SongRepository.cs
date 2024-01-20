using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;

namespace repositories;

public interface ISongRepository{
    Task<IEnumerable<Song>> FindAll();

    Task<Song> FindOne(String id);
    Task<Song> Create(Song song);
    Task<Song> Delete(String id);
    Task<Song> Update(String id, Song song);

}

public class SongRepository : ISongRepository
{
    private readonly IDriver _driver;
    public SongRepository(IDriver driver) {
        _driver = driver;
    }
    public async Task<Song> Create(Song song)
    {
        var session = _driver.AsyncSession();
        return (Song) await session.ExecuteWriteAsync(async (trans)=>{
            var cursor = await trans.RunAsync(@"
                MATCH( author: Author {name: $name})
                CREATE (song:Song {id: $id , title: $title, author: $name})
                MERGE (author)-[:PERFORMS]->(song)
                RETURN song
            ", new { name = song.Author, id = Guid.NewGuid().ToString(), title = song.Title });
            return await cursor.SingleAsync(res => res.AsObject<Song>());            
        });
    }

    public async Task<Song> Delete(String id)
    {
        var session = _driver.AsyncSession();
        return (Song) await session.ExecuteWriteAsync(async (trans)=>{
            var cursor = await trans.RunAsync(@"
                MATCH (song: Song { id: $id })
                OPTIONAL MATCH (song)<-[:PERFORMS]-(authorObj: Author)
                WITH song, song.id AS id, song.title AS title, authorObj.name as author
                DETACH DELETE song
                RETURN id, title, author
            ", new { id });
            return await cursor.SingleAsync(res => res.AsObject<Song>());
        });
    }

    public async Task<IEnumerable<Song>> FindAll()
    {
        var session = _driver.AsyncSession();
        return (IEnumerable<Song>)await session.ExecuteReadAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (s:Song) RETURN s
            ");
            return await cursor.ToListAsync(rec => {
                    return rec.AsObject<Song>();
                ;});
        });
    }

    public async Task<Song> FindOne(String id)
    {
        var session = _driver.AsyncSession();
        return (Song) await session.ExecuteWriteAsync(async (trans)=>{
            var cursor = await trans.RunAsync(@"
                MATCH (song: Song { id: $id })
                RETURN song
            ", new { id });
            return await cursor.SingleAsync(res => res.AsObject<Song>());
        });
    }

    public async Task<Song> Update(String id, Song song)
    {
        var session = _driver.AsyncSession();
        return (Song) await session.ExecuteWriteAsync(async (trans)=>{
            var cursor = await trans.RunAsync(@"
                MATCH (song: Song { id: $id })
                SET song.title = $title
                RETURN song
            ", new { id , title = song.Title });
            return await cursor.SingleAsync(res => res.AsObject<Song>());
        });
    }
}