using Neo4j.Driver;
using repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddSingleton(GraphDatabase.Driver(Environment.GetEnvironmentVariable("NEO4J_URL") ?? "neo4j://localhost:7687"));
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var redisConfiguration = ConfigurationOptions.Parse("localhost:6379");
    return ConnectionMultiplexer.Connect(redisConfiguration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
