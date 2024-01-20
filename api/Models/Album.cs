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
    public string Name { get; set; }
    public string? AuthorName {get; set; }
    public string[]? Genres { get; set; }
}