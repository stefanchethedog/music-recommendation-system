using models;
namespace views;
public class AlbumView : Album {
    public string ArtistName { get; set; }
    public AlbumView(string id, string name, string artistName): base(id, name) {
        ArtistName = artistName;
    }
}