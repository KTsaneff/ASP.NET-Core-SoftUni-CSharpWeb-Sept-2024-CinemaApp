﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    public class CinemaMovie
    {
        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public Guid CinemaId { get; set; }

        public virtual Cinema Cinema { get; set; } = null!;

        public int AvailableTickets { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string Showtimes { get; set; } = "00000";
    }
}
