using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints {
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [
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
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());            
            
        }).WithName(GetGameEndpointName);   

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => 
        {

            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDetailsDto gameDetailsDto = game.ToGameDetailsDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new{id = game.Id}, gameDetailsDto);
        });

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => 
        {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null) 
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();

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
