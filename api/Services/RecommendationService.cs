using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using views;

public class RecommendationService
{
    private readonly IConnectionMultiplexer _redis;

    public RecommendationService(IConnectionMultiplexer redis)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
    }

    public async Task SaveSongsToRedis(List<SongView> songs)
    {
        var database = _redis.GetDatabase();
        var transaction = database.CreateTransaction();

        for (int i = 0; i < songs.Count; i++)
        {
            var redisKey = $"songs:{i + 1:000}";
            
            string description = songs[i].Name + " " + songs[i].Author + " " + songs[i].Album;
            for(int j = 0 ; i < songs[i].Genres.Count; i++){
                description += songs[i].Genres[j];
            }
            
            var obj = new {
                Name = songs[i].Name,
                Author = songs[i].Author,
                Album = songs[i].Album,
                Genres = songs[i].Genres,
                Description = description
            };

            var songObject = JsonConvert.SerializeObject(obj);

            await transaction.SetAddAsync(redisKey, songObject);
        }

        await transaction.ExecuteAsync();
    }
}
