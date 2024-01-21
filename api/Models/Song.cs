namespace models;

public class Song {
    public required string Id { get; set; }
    public required string Name { get; set; }

    public Song(){}

    public Song(string id, string name)
    {
        Id = id;
        Name = name;
    }
}


public class CreateSong
{
    public required string Name { get; set; }
    public required string Author { get; set; }
    public string? Album { get; set; }
    public required List<string> Genres {get; set;} 

    public CreateSong(){}
    public CreateSong(string name, string author, IEnumerable<string> genre, string? album)
    {        
        Name = name;
        Author = author;
        Album = album;
    }
} 