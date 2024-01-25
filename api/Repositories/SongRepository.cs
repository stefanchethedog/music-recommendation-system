using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;
using views;

namespace repositories;

public interface ISongRepository
{
    Task<IEnumerable<SongView>> FindAll();
    Task<SongView?> FindOne(String id);
    Task<SongView?> Create(CreateSong song);
    Task<SongView?> Delete(String id);
    Task<SongView?> Update(
        string id,
        string newName,
        string newAuthor,
        string? newAlbum,
        List<String> newGenres
    );
    Task<SongView?> UpdateArtist(string id, string newArtist);
    Task<SongView?> UpdateAlbum(string id, string? newAlbum);
    Task<SongView?> UpdateName(string id, string newName);
    Task<SongView?> UpdateGenres(string id, List<string> newGenres);
    Task<SongView?> AddGenre(string id, string genre);
    Task<SongView?> RemoveGenre(string id, string genre);
}

public class SongRepository : ISongRepository
{
    private readonly IDriver _driver;

    public SongRepository(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<SongView?> Create(CreateSong song)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
            MATCH (artist: Artist { name: $authorName })
            OPTIONAL MATCH (album: Album { name: $albumName })
            CREATE (song: Song {id: $id, name: $songName})

            FOREACH (genName IN $genresList |
                MERGE (genre: Genre {name: genName})
                MERGE (genre)<-[:IN_GENRE]-(song)
            )
            MERGE (artist)-[:PERFORMS]->(song)

            WITH song, artist, album
            WHERE album IS NOT NULL
                MERGE (album)<-[:IN_ALBUM]-(song)
            WITH song, artist, album

            UNWIND $genresList AS genName

            RETURN 
                song.id AS id,
                song.name AS name,
                album.name AS album,
                artist.name AS author,
                COLLECT(genName) AS genres;

        ",
                    new
                    {
                        authorName = song.Author,
                        albumName = song.Album,
                        id = Guid.NewGuid().ToString(),
                        songName = song.Name,
                        genresList = song.Genres,
                    }
                );

                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> Delete(String id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                MATCH (artist: Artist)-[:PERFORMS]->(song)
                MATCH (genre: Genre)<-[:IN_GENRE]-(song)
                OPTIONAL MATCH (album: Album)<-[:IN_ALBUM]-(song)

                WITH song, song.id AS id, song.name AS name, artist, genre, album
                DETACH DELETE song

                RETURN 
                    id,
                    name,
                    artist.name AS author,
                    album.name AS album,
                    COLLECT(genre.name) AS genres; 
            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<IEnumerable<SongView>> FindAll()
    {
        var session = _driver.AsyncSession();
        return (IEnumerable<SongView>)
            await session.ExecuteReadAsync(
                async (trans) =>
                {
                    var cursor = await trans.RunAsync(
                        @"
                MATCH (song:Song)
                MATCH (author:Artist)-[:PERFORMS]->(song)
                MATCH (genre:Genre)<-[:IN_GENRE]-(song)
                OPTIONAL MATCH (album:Album)<-[:IN_ALBUM]-(song)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    author.name AS author,
                    album.name AS album,
                    COLLECT(genre.name) AS genres;
            "
                    );
                    return await cursor.ToListAsync(rec =>
                    {
                        return rec.AsObject<SongView>();
                        ;
                    });
                }
            );
    }

    public async Task<SongView?> FindOne(String id)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                MATCH (artist: Artist)-[:PERFORMS]->(song)
                MATCH (genre: Genre)<-[:IN_GENRE]-(song)
                OPTIONAL MATCH (album: Album)<-[:IN_ALBUM]-(song)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    album.name AS album,
                    COLLECT(genre.name) AS genres;
            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> Update(
        string id,
        string newName,
        string newAuthor,
        string? newAlbum,
        List<string> newGenres
    )
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                WITH song
                SET song.name = $newName
                
                WITH song
                OPTIONAL MATCH (song)<-[relationship_artist:PERFORMS]-(artist: Artist)
                DELETE relationship_artist

                WITH song
                OPTIONAL MATCH (song)<-[relationship_album:IN_ALBUM]-(album: Album)
                DELETE relationship_album

                WITH song
                MATCH (song)-[relationship_genre:IN_GENRE]->(genre: Genre)
                DELETE relationship_genre

                WITH song 
                MERGE (newArtist: Artist { name: $newAuthor })
                MERGE (song)<-[:PERFORMS]-(newArtist)

                MERGE (newAlbum: Album { name: $newAlbum })
                MERGE (song)-[:IN_ALBUM]->(newAlbum)

                WITH song, newAlbum, newArtist

                UNWIND $newGenres AS genName
                MERGE (genre: Genre { name: genName })
                MERGE (song)-[:IN_GENRE]->(genre)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    newArtist.name AS author,
                    newAlbum.name AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new
                    {
                        id,
                        newName,
                        newAlbum,
                        newAuthor,
                        newGenres
                    }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> UpdateArtist(string id, string newAuthor)
    {
        var session = _driver.AsyncSession();
        var objectId = await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                WITH song
                
                MATCH (song)<-[relationship_artist:PERFORMS]-(artist: Artist)
                DELETE relationship_artist

                WITH song

                MERGE (newArtist: Artist { name: $newAuthor })
                MERGE (song)<-[:PERFORMS]-(newArtist)

                WITH newArtist, song

                RETURN
                    song.id AS id,
                    song.name AS name,
                    newArtist.name AS author;

            ",
                    new { id, newAuthor }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
        if (objectId == null)
            return null;

        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song:Song { id: $id })
                MATCH (song)-[:IN_GENRE]->(genre: Genre)
                MATCH (song)<-[:PERFORMS]-(artist: Artist)
                OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    CASE WHEN album IS NOT NULL THEN album.name ELSE null END AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> UpdateAlbum(string id, string? newAlbum)
    {
        var session = _driver.AsyncSession();
        var objectId = await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                    MATCH (song: Song { id: $id })
                    WITH song

                    OPTIONAL MATCH (song)-[relationship_album:IN_ALBUM]->(album: Album)
                    DELETE relationship_album

                    WITH song

                    FOREACH(ignore IN CASE WHEN $newAlbum IS NOT NULL THEN [1] ELSE [] END |
                        MERGE (album: Album { name: $newAlbum })
                        MERGE (song)-[:IN_ALBUM]->(album)
                    )

                    WITH song

                    RETURN
                        song.id AS id,
                        song.name AS name,
                        $newAlbum AS album;

                ",
                    new { id, newAlbum }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
        if (objectId == null)
            return null;

        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                    MATCH (song:Song { id: $id })
                    MATCH (song)-[:IN_GENRE]->(genre: Genre)
                    MATCH (song)<-[:PERFORMS]-(artist: Artist)
                    OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)

                    RETURN
                        song.id AS id,
                        song.name AS name,
                        artist.name AS author,
                        CASE WHEN album IS NOT NULL THEN album.name ELSE null END AS album,
                        COLLECT(genre.name) AS genres;

                ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> UpdateName(string id, string newName)
    {
        var session = _driver.AsyncSession();
        var objectId = await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                WITH song
                SET song.name = $newName
                
                WITH song

                MATCH(song)-[:IN_GENRE]->(genre: Genre)
                MATCH(song)<-[:PERFORMS]-(artist: Artist)
                OPTIONAL MATCH(song)-[:IN_ALBUM]->(album:Album)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    album.name AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new { id, newName }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );

        if (objectId == null)
            return null;

        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song:Song { id: $id })
                MATCH (song)-[:IN_GENRE]->(genre: Genre)
                MATCH (song)<-[:PERFORMS]-(artist: Artist)
                OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    CASE WHEN album IS NOT NULL THEN album.name ELSE null END AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> UpdateGenres(string id, List<string> newGenres)
    {
        var session = _driver.AsyncSession();
        var objectId = await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                WITH song
                
                MATCH (song)-[relationship_genre:IN_GENRE]->(genre: Genre)
                DELETE relationship_genre
                WITH song 

                UNWIND $newGenres AS genName
                MERGE (genre: Genre { name: genName })
                MERGE (song)-[:IN_GENRE]->(genre)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    COLLECT(genre.name) AS genres;
            ",
                    new { id, newGenres }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );

        if (objectId == null)
            return null;

        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song:Song { id: $id })
                MATCH (song)-[:IN_GENRE]->(genre: Genre)
                MATCH (song)<-[:PERFORMS]-(artist: Artist)
                OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    CASE WHEN album IS NOT NULL THEN album.name ELSE null END AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> AddGenre(string id, string genre)
    {
        var session = _driver.AsyncSession();
        var objectId = await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song: Song { id: $id })
                MATCH(genre: Genre { name: $genre })
                MERGE(genre)<-[:IN_GENRE]-(song)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    COLLECT(genre.name) AS genres;
            ",
                    new { id, genre }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
        if (objectId == null)
            return null;

        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song:Song { id: $id })
                MATCH (song)-[:IN_GENRE]->(genre: Genre)
                MATCH (song)<-[:PERFORMS]-(artist: Artist)
                OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    CASE WHEN album IS NOT NULL THEN album.name ELSE null END AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }

    public async Task<SongView?> RemoveGenre(string id, string genre)
    {
        var session = _driver.AsyncSession();
        var objectId = await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song :Song { id: $id })
                    -[song_genre_relationship:IN_GENRE]->
                (genre :Genre { name: $genre })
                DELETE song_genre_relationship
                RETURN
                    song

            ",
                    new { id, genre }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
        if (objectId == null)
            return null;

        return await session.ExecuteReadAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (song:Song { id: $id })
                MATCH (song)-[:IN_GENRE]->(genre: Genre)
                MATCH (song)<-[:PERFORMS]-(artist: Artist)
                OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)

                RETURN
                    song.id AS id,
                    song.name AS name,
                    artist.name AS author,
                    CASE WHEN album IS NOT NULL THEN album.name ELSE null END AS album,
                    COLLECT(genre.name) AS genres;

            ",
                    new { id }
                );
                if (await cursor.FetchAsync())
                {
                    return cursor.Current.AsObject<SongView>();
                }
                else
                {
                    return null;
                }
            }
        );
    }
}
