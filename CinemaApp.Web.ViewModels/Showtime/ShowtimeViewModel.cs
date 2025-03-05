using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Showtime
{
    public class ShowtimeViewModel
    {
        [Required]
        public Guid CinemaId { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        public string Title { get; set; } = string.Empty;


        /// <summary>
        /// A list of 5 integers representing the showtimes: 
        /// [12PM, 3PM, 6PM, 8PM, 10PM] where 1 = selected, 0 = not selected.
        /// </summary>
        [Required]
        [MaxLength(5)]
        public List<int> Showtimes { get; set; } = new List<int>(new int[5]);
    }
}
