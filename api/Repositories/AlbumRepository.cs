using models;
using views;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;

namespace repositories;

public interface IAlbumRepository
{
    Task<IEnumerable<AlbumView>> FindAll();
    Task<Album?> FindOne(string id);
    Task<Album> Create(CreateAlbum user);
    Task<Album?> Delete(string id);
    Task<Album?> Update(string id, UpdateAlbum albumInfo);
}

public class AlbumRepository : IAlbumRepository
{
    private readonly IDriver _driver;
    public AlbumRepository(IDriver driver)
    {
        _driver = driver;
    }
    public async Task<Album> Create(CreateAlbum album)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans =>
        {
            var id = Guid.NewGuid().ToString();
            var cursor = await trans.RunAsync(@"
                MATCH (artist:Artist {name: $artistName})
                CREATE (artist)-[:CREATED]->(album:Album {id: $id, name: $name})

                FOREACH (genreName in $genres |
                    MERGE (genre:Genre {name: genreName})
                    MERGE (genre)<-[:IN_GENRE]-(album)
                )

                FOREACH (songName in $songs |
                    MERGE (song:Song {name: songName})
                    MERGE (song)-[:IN_ALBUM]->(album)
                )

                RETURN album;
                ",
                new
                {
                    id,
                    name = album.Name,
                    genres = album.Genres,
                    artistName = album.AuthorName,
                    songs = album.Songs
                });
            return await cursor.SingleAsync(rec => rec.AsObject<Album>());
        });
    }

    public async Task<Album?> Delete(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async trans =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (a:Album {id: $id}) 
                WITH a, a.id AS id, a.name AS name 
                DETACH DELETE a RETURN id, name;
            ", new { id });
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<Album>();
            }
            return null;
        });
    }

    public async Task<IEnumerable<AlbumView>> FindAll()
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (album:Album)<-[:CREATED]-(artist:Artist)
                MATCH (songs:Song)-[:IN_ALBUM]->(album)
                WITH album.id AS id, album.name AS name, artist.name AS artistName, COLLECT(songs.name) AS songNames
                RETURN {id: id, songs: songNames, name: name, artistName: artistName} AS a;
            ");
            return await cursor.ToListAsync(rec =>
            {
                return rec.AsObject<AlbumView>();
            });
        });
    }

    public async Task<Album?> FindOne(string id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (album:Album) WHERE album.id = $id 
                MATCH (songs:Song)-[:IN_ALBUM]->(album)
                MATCH (artist:Artist)-[:CREATED]->(album)
                WITH album.id AS id, album.name AS name, artist.name AS artistName, COLLECT(songs.name) AS songNames
                RETURN {id: id, songs: songNames, name: name, artistName: artistName} AS a;
            ", new { id });
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<AlbumView>();
            }
            else { return null; }
        });
    }

    public async Task<Album?> Update(string id, UpdateAlbum albumInfo)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                OPTIONAL MATCH (a:Album {id: $id}) 
                MATCH (s:Song)-[in_album:IN_ALBUM]->(a)
                MATCH (g:Genre)-[in_genre:IN_GENRE]-(a)
                MATCH (artist:Artist)-[created:CREATED]->(a)

                WITH *, collect(DISTINCT s.name) as songNames, collect(DISTINCT g.name) as genreNames, a WHERE a IS NOT NULL
                DETACH DELETE in_album
                DETACH DELETE in_genre
                DETACH DELETE created
                MERGE (newArtist: Artist {name: COALESCE($artistName, artist.name)})
                MERGE (newArtist)-[:CREATED]->(a)
                FOREACH( genreName in COALESCE($genres, genreNames) |
                    MERGE (genre:Genre{name: genreName})
                    MERGE (a)-[:IN_GENRE]-(genre)
                )
                FOREACH( songName in COALESCE($songs, songNames) |
                    MERGE (song:Song{ name: songName})
                    MERGE (a)<-[:IN_ALBUM]-(song)
                )
                SET a.name = COALESCE($name, a.name) RETURN DISTINCT a; 

            ", new { id, name = albumInfo.Name, artistName = albumInfo.AuthorName, songs = albumInfo.Songs, genres = albumInfo.Genres});
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<Album>();
            }
            else { return null; }
        });
    }

}