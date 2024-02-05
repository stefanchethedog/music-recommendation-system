namespace models;

public class RedisSong
{
  public string Id { get; set; }
  public float Score { get; set; }
  public string Name { get; set; }
  public string Author { get; set; }
  public string Album { get; set; }
  public RedisSong() { }
}
