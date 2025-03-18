using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

app.MapGet("/", () => "Hello World!");

app.Run();
