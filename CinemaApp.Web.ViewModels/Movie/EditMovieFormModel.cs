namespace CinemaApp.Web.ViewModels.Movie
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    public class EditMovieFormModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Genre is required.")]
        [MinLength(3)]
        [MaxLength(50)]
        public string Genre { get; set; } = null!;

        [Required(ErrorMessage = "Release Date is required.")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Director name is required.")]
        [MinLength(3)]
        [MaxLength(50)]
        public string Director { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required.")]
        [Range(30, 300, ErrorMessage = "Duration must be between 30 and 300 minutes.")]
        public int Duration { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [MaxLength(300)]
        public string? ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, EditMovieFormModel>()
                .ReverseMap();
        }
    }
}
