namespace models;

public class Genre {
    public string Id { get; set; }
    public string Name { get; set; }
    public Genre(string id, string name) {
        Id = id;
        Name = name;
    }
}

public class CreateGenre {
    public string Name { get; set; }
}