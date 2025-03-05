using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Showtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("Manager/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShowtimeApiController : ControllerBase
    {       
        private readonly ICinemaService _cinemaService;

        public ShowtimeApiController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [HttpGet("GetMoviesByCinema/{cinemaId}")]
        public async Task<IActionResult> GetMoviesByCinema(Guid cinemaId)
        {
            var movies = await _cinemaService.GetMoviesWithShowtimesAsync(cinemaId);
            if (movies == null)
            {
                return NotFound("Cinema not found.");
            }
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShowtimes([FromBody] ShowtimeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cinemaService.UpdateShowtimeAsync(model);
            if (!result)
            {
                return BadRequest("Failed to update showtimes. Please try again.");
            }
            return Ok(new { message = "Showtimes updated successfully." });
        }
    }
}
