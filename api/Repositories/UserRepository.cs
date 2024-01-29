using models;
using Neo4j.Driver;
using Neo4j.Driver.Preview.Mapping;
using views;

namespace repositories;

public interface IUserRepository
{
  Task<IEnumerable<User>> FindAll();
  Task<User?> FindOne(string id);
    Task<User?> FindUserByUsername(string username);
  Task<User> Create(User user);
  Task<User?> Delete(string id);
  Task<User?> Update(string id, string username);
  Task<User?> AddLikesSong(string id, string songName);
  Task<User?> FollowUser(string id, string username);
  Task<List<SongView>?> FindOtherUsersSongs(string id);
  Task<List<SongView>?> FindSongsByTheFollowedUsers(string id);
    Task<List<SongView>?> GetLikedSongs(string id);
}

public class UserRepository : IUserRepository
{
  private readonly IDriver _driver;
  private readonly RecommendationService _recService;
  public UserRepository(IDriver driver, RecommendationService recService)
  {
    _driver = driver;
    _recService = recService;
  }
  public async Task<User> Create(User user)
  {
    var session = _driver.AsyncSession();
    return await session.ExecuteWriteAsync(async trans =>
    {
      var cursor = await trans.RunAsync(@"
                CREATE (u:User {id: $id, username: $username}) 
                RETURN u {.id, .username};
            ", new { id = user.Id, username = user.Username });
      return await cursor.SingleAsync(rec => rec.AsObject<User>());
    });
  }

  public async Task<User?> Delete(string id)
  {
    var session = _driver.AsyncSession();
    return await session.ExecuteWriteAsync(async trans =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (u:User {id: $id}) 
                WITH u, u.id AS id, u.username AS username 
                DETACH DELETE u RETURN id, username;
            ", new { id });
      if (await cursor.FetchAsync())
      {
        return cursor.Current.AsObject<User>();
      }
      return null;
    });
  }

  public async Task<IEnumerable<User>> FindAll()
  {
    var session = _driver.AsyncSession();
    return await session.ExecuteReadAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (u:User) 
                RETURN u { .id, .username };
            ");
      return await cursor.ToListAsync(rec =>
          {
            return rec.AsObject<User>();
            ;
          });
    });
  }

    public async Task<List<SongView>?> GetLikedSongs(string id)
    {
         var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(
            async (trans) =>
            {
                var cursor = await trans.RunAsync(
                    @"
                MATCH (user: User { id: $id })
                MATCH (song: Song)<-[:USER_LIKES_SONG]-(user)
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
                return await cursor.ToListAsync(rec =>
                {
                    return rec.AsObject<SongView>();
                    ;
                });
            }
        );
    }
  public async Task<User?> FindOne(string id)
  {
    var session = _driver.AsyncSession();
    return await session.ExecuteReadAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (u:User) WHERE u.id = $id 
                RETURN u {.id, .username}
            ", new { id });
      if (await cursor.FetchAsync())
      {
        return cursor.Current.AsObject<User>();
      }
      else { return null; }
    });
  }

    public async Task<User?> FindUserByUsername(string username)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async (trans) =>
        {
            var cursor = await trans.RunAsync(@"
                MATCH (u:User) WHERE u.username = $username 
                RETURN u {.id, .username}
            ", new { username });
            if (await cursor.FetchAsync())
            {
                return cursor.Current.AsObject<User>();
            }
            else { return null; }
        });
    }

  public async Task<User?> Update(string id, string username)
  {
    var session = _driver.AsyncSession();
    return await session.ExecuteWriteAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                OPTIONAL MATCH (user:User {id: $id}) 
                WITH user WHERE user IS NOT NULL 
                SET user.username = $username RETURN user {.id, .username}
            ", new { id, username });
      if (await cursor.FetchAsync())
      {
        return cursor.Current.AsObject<User>();
      }
      else { return null; }
    });
  }

  public async Task<User?> AddLikesSong(string id, string songName)
  {
    var session = _driver.AsyncSession();
    return await session.ExecuteWriteAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (user: User {id: $id}) 
                MATCH (song: Song {name: $songName})
                MERGE(song)<-[:USER_LIKES_SONG]-(user)

                RETURN
                    user;    
            ", new { id, songName });
      if (await cursor.FetchAsync())
      {
        return cursor.Current.AsObject<User>();
      }
      else { return null; }
    });
  }

    public async Task<User?> FollowUser(string id, string username)
    {
        var session = _driver.AsyncSession();
        return await session.ExecuteWriteAsync(async (trans) => {
            var cursor = await trans.RunAsync(@"
                MATCH (user: User {id: $id})
                MATCH (userToFollow: User {username : $username})
                MERGE (user)-[:FOLLOWS]->(userToFollow)
                RETURN user, userToFollow;   
            ", new { id, username });
      if (await cursor.FetchAsync())
      {
        return cursor.Current.AsObject<User>();
      }
      else { return null; }
    });
  }

  public async Task<List<SongView>?> FindOtherUsersSongs(string id)
  {
    var session = _driver.AsyncSession();
    List<Song> queryResult = await session.ExecuteReadAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (user:User {id: $id})-[:USER_LIKES_SONG]->(commonSong:Song)
                WITH user, commonSong
                MATCH (otherUser:User)-[:USER_LIKES_SONG]->(commonSong)
                WHERE otherUser.id <> $id
                MATCH (otherUser)-[:USER_LIKES_SONG]->(recommendedSong:Song)
                WHERE NOT (user)-[:USER_LIKES_SONG]->(recommendedSong)
                RETURN DISTINCT recommendedSong
            ", new { id });
      return await cursor.ToListAsync(rec =>
      {
        return rec.AsObject<Song>();
        ;
      });
    });

    var songIds = queryResult.Select(song => song.Id).ToList();
    if (songIds.Count == 0) return null;

    return await session.ExecuteReadAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (song:Song)
                WHERE song.id IN $songIds
                OPTIONAL MATCH (song)<-[:PERFORMS]-(artist:Artist)
                OPTIONAL MATCH (song)-[:SONG_BELONGS_TO_ALBUM]->(album:Album)
                OPTIONAL MATCH (song)-[:IN_GENRE]->(genre:Genre)
                RETURN
                    song.id AS Id,
                    song.name AS Name,
                    artist.name AS Author,
                    album.name AS Album,
                    COLLECT(genre.name) AS Genres
            ", new { songIds });
      return await cursor.ToListAsync(rec =>
      {
        return rec.AsObject<SongView>();
      });
    });
  }

  public async Task<List<SongView>?> FindSongsByTheFollowedUsers(string id)
  {
    var session = _driver.AsyncSession();
    List<Song> queryResult = await session.ExecuteReadAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (user: User {id: $id})-[:FOLLOWS]->(followingUser: User)
                WITH user, followingUser
                MATCH (followingUser)-[:USER_LIKES_SONG]->(song: Song)
                WHERE NOT (user)-[:USER_LIKES_SONG]->(song)
                RETURN DISTINCT song
            ", new { id });
      return await cursor.ToListAsync(rec =>
      {
        return rec.AsObject<Song>();
        ;
      });
    });

    var songIds = queryResult.Select(song => song.Id).ToList();
    if (songIds.Count == 0) return null;

    return await session.ExecuteReadAsync(async (trans) =>
    {
      var cursor = await trans.RunAsync(@"
                MATCH (song:Song)
                WHERE song.id IN $songIds
                OPTIONAL MATCH (song)<-[:PERFORMS]-(artist:Artist)
                OPTIONAL MATCH (song)-[:IN_ALBUM]->(album:Album)
                OPTIONAL MATCH (song)-[:IN_GENRE]->(genre:Genre)
                RETURN
                    song.id AS Id,
                    song.name AS Name,
                    artist.name AS Author,
                    album.name AS Album,
                    COLLECT(genre.name) AS Genres
            ", new { songIds });
      var songs = await cursor.ToListAsync(rec =>
      {
        return rec.AsObject<SongView>();
      });
      var recommendations = await _recService.GetRedisRecommendations(songs);
      return songs;
    });
  }
}
