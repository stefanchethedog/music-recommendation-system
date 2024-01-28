using etackExchange.Redis;
using Newtonsoft.Json;
using views;
using System.Text;

public class RecommendationService
{
  private readonly IConnectionMultiplexer _redis;

  public RecommendationService(IConnectionMultiplexer redis)
  {
    _redis = redis ?? throw new ArgumentNullException(nameof(redis));
  }

  public async Task<string> SaveSongsToRedis(List<SongView> songs)
  {
    var database = _redis.GetDatabase();
    var transaction = database.CreateTransaction();
    foreach (var (song, idx) in songs.Select((song, idx) => (song, idx)))
    {
      var redisKey = $"songs:{idx + 1:000}";
      string description = song.Name + " " + song.Author + " " + song.Album + " ";
      foreach (var genre in song.Genres)
      {
        description = description + genre + " ";
      }

      var obj = new
      {
        Name = song.Name,
        Author = song.Author,
        Album = song.Album,
        Genres = song.Genres,
        Description = description
      };

      var songObject = JsonConvert.SerializeObject(obj);

      await transaction.SetAddAsync(redisKey, songObject);
    }

    await transaction.ExecuteAsync();
    //var serializedSongs = JsonConvert.SerializeObject(songs);
    //var content = new StringContent(serializedSongs, Encoding.UTF8, "application/json");
    //HttpClient httpClient = new HttpClient();
    //var response = await httpClient.PostAsync("localhost:6666/recommendations", content);
    //var responseContent = await response.Content.ReadAsStringAsync();
    //var data = JsonConvert.DeserializeAnonymousType(responseContent, new { vector_score = "", id = "", name = "", author = "", genres = new List<string>() });
    return "";
  }
}
