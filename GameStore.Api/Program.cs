using GameStore.Api.Dtos;
using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new(1,
        "Street Fighter II",
        "Fighting",
        899.00M,
        new DateOnly(1992, 7, 15)),
    new(2,
        "Final Fantasy XIV",
        "Roleplaying",
        245.00M,
        new DateOnly(2010, 9, 20)),
    new(3,
        "Fifa 23",
        "Sports",
        1399.00M,
        new DateOnly(2022, 9, 27))
];

// GET games
app.MapGet("games", () => games);

// GET game/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

// POST games
app.MapPost("games", (CreateGameDto newGame) => {
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new{id = game.Id}, game);
});

// PUT games
app.MapPut("games/{id}", (int id, UpdateDto updatedGame) => {
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

app.Run();
