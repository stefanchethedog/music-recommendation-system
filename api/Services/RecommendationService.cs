using System.Text;
using Newtonsoft.Json;
using StackExchange.Redis;
using views;

public class RecommendationService
{
  private readonly IConnectionMultiplexer _redis;

  public RecommendationService(IConnectionMultiplexer redis)
  {
    _redis = redis ?? throw new ArgumentNullException(nameof(redis));
  }

  public async Task<string> GetRedisRecommendations(List<SongView> songs)
  {
    var serializedSongs = JsonConvert.SerializeObject(songs);
    var content = new StringContent(serializedSongs, Encoding.UTF8, "application/json");
    HttpClient httpClient = new HttpClient();
    var response = await httpClient.PostAsync("http://localhost:6666/recommendations", content);
    var responseContent = await response.Content.ReadAsStringAsync();
    //TODO: deserialization fix
    //var data = JsonConvert.DeserializeAnonymousType(responseContent, new { id = "", album = "", name = "", author = "", genres = new List<string>() });
    return "";
  }
}
