using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class UpdateDto (
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)]string Genre,
    [Range(1, 5000)]decimal Price,
    DateOnly ReleaseDate
);
