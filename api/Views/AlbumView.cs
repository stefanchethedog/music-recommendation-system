using models;
namespace views;
public class AlbumView : Album
{
    public string ArtistName { get; set; }
    public List<string>? Songs { get; set; }
    public AlbumView(string id, string name, string artistName, List<string> songs) : base(id, name)
    {
        ArtistName = artistName;
        Songs = songs;
    }
}