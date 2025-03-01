namespace CinemaApp.Web.ViewModels.Movie
{
    using CinemaApp.Services.Mapping;
    using System;
    using CinemaApp.Data.Models;

    public class MovieIndexViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string Director { get; set; } = null!;

        public int Duration { get; set; }

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public void CreateMappings(AutoMapper.IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, MovieIndexViewModel>()
                .ForMember(
                    destination => destination.ReleaseDate,
                    opts => opts.MapFrom(origin => origin.ReleaseDate.ToString("yyyy-MM-dd")));
        }
    }
}
