using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints {
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
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

    public static RouteGroupBuilder MapGamesEndpoint(this WebApplication app) {

        var group = app.MapGroup("games")
                    .WithParameterValidation();
                       
        // GET /games
        group.MapGet("/", () => games);

        // GET /game/1
        group.MapGet("/{id}", (int id) => games.Find(game => game.Id == id))
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => 
        {
            Game game = new() 
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, new{id = game.Id}, game);
        });

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateDto updatedGame) => {
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

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) => {
            games.RemoveAll(game => game.Id == id);
            
            return Results.NoContent();
        });

        return group;
    }
}
