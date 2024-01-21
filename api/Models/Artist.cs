namespace models;

public class Artist {
    public string Id { get; set; }
    public string Name { get; set; }
    public Artist(string id, string name) {
        Id = id;
        Name = name;
    }
}

public class CreateArtist {
    public string Name { get; set; }
    public CreateArtist(string name){
        Name = name;
    }
}