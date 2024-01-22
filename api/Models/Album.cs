namespace models;

public class Album {
    public string Id { get; set; }
    public string Name { get; set; }
    public Album(string id, string name) {
        Id = id;
        Name = name;
    }
}
public class CreateAlbum {
    public required string Name { get; set; }
    public required string AuthorName {get; set; }
    public required IEnumerable<string> Genres { get; set; }
    public required IEnumerable<string> Songs { get; set; }
}

public class UpdateAlbum {
    public string? Name { get; set; }
    public string? AuthorName { get; set; }
    public IEnumerable<string>? Genres { get; set; }
    public IEnumerable<string>? Songs { get; set; }
}