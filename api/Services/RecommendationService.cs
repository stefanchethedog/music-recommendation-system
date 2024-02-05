using System.Text;
using models;
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

  public async Task<List<RedisSong>> GetRedisRecommendations(List<string> queries, List<SongView> mySongs)
  {
    var x = new { queries = queries, songs = mySongs };
    var payload = JsonConvert.SerializeObject(x);
    var content = new StringContent(payload, Encoding.UTF8, "application/json");
    HttpClient httpClient = new HttpClient();
    var response = await httpClient.PostAsync("http://localhost:6666/recommendations", content);
    var responseContent = await response.Content.ReadAsStringAsync();
    List<RedisSong> results = JsonConvert.DeserializeObject<List<RedisSong>>(responseContent)!;
    return results;
  }
}
