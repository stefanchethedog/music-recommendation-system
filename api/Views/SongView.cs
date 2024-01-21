namespace views;

public class SongView 
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string? Album { get; set; }
    public List<String> Genres {get; set;}

    public SongView(){}
    public SongView(String id, String name, String author, String? album, List<String> genres){
        Id = id;
        Name = name;
        Author = author;
        Album = album;
        Genres = genres;
    }
};
